using System.Diagnostics;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentDate;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Mqtt;

/// <summary>
///     Net2HassMqtt's MQTT client.
/// </summary>
/// <seealso cref="https://home-assistant-china.github.io/docs/mqtt/discovery/" />
/// <seealso cref="https://www.zigbee2mqtt.io/guide/usage/mqtt_topics_and_messages.html" />
internal sealed partial class Net2HassMqttClient : ObservableObject, INet2HassMqttClient, IDisposable
{
    private readonly CancellationTokenSource _cancellationSource;
    private readonly ILogger _logger;
    private readonly IManagedMqttClient _mqttClient;
    private readonly ManagedMqttClientOptions _mqttClientOptions;
    private readonly Dictionary<string, IMqttSubscriber> _subscribersByNodeId = new();

    [ObservableProperty]
    private bool _isConnected;

    internal Net2HassMqttClient(IManagedMqttClient mqttClient, ManagedMqttClientOptions mqttClientOptions, ILogger logger)
    {
        _cancellationSource = new CancellationTokenSource();
        _mqttClient = mqttClient;
        _mqttClientOptions = mqttClientOptions;
        _logger = logger;
        _mqttClient.ConnectedAsync += OnMqttConnectedAsync;
        _mqttClient.DisconnectedAsync += OnMqttDisconnectedAsync;
        var mqttClientId = mqttClientOptions.ClientOptions.ClientId!;
        Discovery = new HassDiscoveryClient(mqttClientId, this);
        _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceived;
        ClientMqttId = mqttClientId;
        IsConnected = _mqttClient.IsConnected;
        OnConnectedAsync += DoNothingEventHandler;
        OnDisconnectedAsync += DoNothingEventHandler;
    }

    public void Dispose()
    {
        OnConnectedAsync -= DoNothingEventHandler;
        OnDisconnectedAsync -= DoNothingEventHandler;
        _mqttClient.Dispose();
    }

    public string ClientMqttId { get; }

    public IHassMqttDiscoveryClient Discovery { get; }

    public event HassMqttClientConnectionEventHandlers.OnConnectionChangedHandlerAsync OnConnectedAsync;

    public event HassMqttClientConnectionEventHandlers.OnConnectionChangedHandlerAsync OnDisconnectedAsync;

    public async Task PublishAsync(MqttTopic topic, string payload)
    {
        if (!_mqttClient.IsConnected)
        {
            _logger.LogWarning(LoggingEvents.MqttConnection, "Unable to publish topic '{Topic} as MQTT client is not connected.", topic.ToString());
            return;
        }

        _logger.LogInformation(LoggingEvents.EntityState, "Publishing topic '{Topic}.", topic.ToString());

        var message = new MqttApplicationMessageBuilder()
                      .WithTopic(topic.ToString())
                      .WithPayload(payload)
                      .WithRetainFlag()
                      .WithQualityOfServiceLevel(0)
                      .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                      .Build();

        try
        {
            await _mqttClient.InternalClient!.PublishAsync(message, _cancellationSource.Token);
        }
        catch (Exception exception)
        {
            _logger.LogError(LoggingEvents.EntityState, exception, "Exception when publishing topic '{Topic}.", topic.ToString());
            throw;
        }
    }

    public async Task PublishStatusAsync<T>(TopicBuilder topicBuilder, T status)
    {
        var payloadJson = JsonSerializer.Serialize(status, MqttConstants.MqttJsonSerialiseOptions);
        await PublishAsync(topicBuilder.WithBaseTopic(ClientMqttId).BuildStateTopic(), payloadJson);
    }
    
    public async Task PublishCommandAsync<T>(TopicBuilder topicBuilder, T status)
    {
        var payloadJson = JsonSerializer.Serialize(status, MqttConstants.MqttJsonSerialiseOptions);
        await PublishAsync(topicBuilder.WithBaseTopic(ClientMqttId).BuildCommandTopic(), payloadJson);
    }

    public async Task StartAsync()
    {
        _logger.LogInformation(LoggingEvents.MqttConnection, "Starting MQTT client.");
        await _mqttClient.StartAsync(_mqttClientOptions);
    }

    public async Task StopAsync(bool cleanDisconnect = true)
    {
        _logger.LogInformation(LoggingEvents.MqttConnection, "Stopping MQTT client.");

        if (IsConnected)
        {
            await PublishLastWillMessage();
            await Task.Delay(MqttConstants.DelayAfterPublishingLastWill);
            await _cancellationSource.CancelAsync();
        }

        await _mqttClient.StopAsync(cleanDisconnect);
    }

    public async Task SubscribeToSetCommandsAsync(string deviceId, IMqttSubscriber subscriber)
    {
        _subscribersByNodeId.Add(deviceId, subscriber);

        var commandTopic = new TopicBuilder().WithBaseTopic(ClientMqttId)
                                      .WithAnyComponent()
                                      .WithNodeId(deviceId)
                                      .WithAnyObjectId()
                                      .WithAction(TopicAction.SetCmd)
                                      .BuildCommandTopic();

        var topicText = commandTopic.ToString();
        _logger.LogInformation(LoggingEvents.EntityState, "Subscribing to topic: {Topic}", topicText);

        await _mqttClient.SubscribeAsync(topicText);
    }

    public async Task<bool> WaitForConnection(FluentTimeSpan seconds)
    {
        var stopwatch = Stopwatch.StartNew();
        while (!IsConnected && stopwatch.Elapsed < seconds)
        {
            await Task.Delay(10.Milliseconds(), _cancellationSource.Token);
        }

        if (IsConnected)
        {
            _logger.LogInformation(LoggingEvents.MqttConnection, "Connected to the MQTT broker.");
            return true;
        }

        _logger.LogWarning(LoggingEvents.MqttConnection, "Unable to connect to the MQTT broker.");
        return false;
    }

    private static Task DoNothingEventHandler(EventArgs args)
    {
        // do nothing - only to silence event null warnings
        return Task.CompletedTask;
    }

    private Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs arg)
    {
        try
        {
            var message = arg.ApplicationMessage;
            var receivedMessage = MqttPayloadParser.Parse(message);

            if (!receivedMessage.IsValid)
            {
                _logger.LogError(LoggingEvents.MqttReceived, "Received an invalid MQTT message.");
                return Task.CompletedTask;
            }

            var baseTopic = receivedMessage.BaseTopic;
            if (baseTopic != ClientMqttId)
            {
                _logger.LogError(LoggingEvents.MqttReceived,
                                 "Received MQTT message base topic '{BaseTopic}' is not invalid. Expected '{MqttClientId}'.", baseTopic,
                                 ClientMqttId);
                return Task.CompletedTask;
            }

            var nodeId = receivedMessage.NodeId;
            _logger.LogInformation(LoggingEvents.MqttReceived, "Received MQTT message for node {NodeId}.", nodeId);
            if (_subscribersByNodeId.TryGetValue(nodeId, out var subscriber))
            {
                subscriber.OnReceived(receivedMessage);
            }

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            _logger.LogError(LoggingEvents.MqttReceived, exception, "Exception when processing received MQTT message.");
            throw;
        }
    }

    private async Task OnMqttConnectedAsync(MqttClientConnectedEventArgs arg)
    {
        IsConnected = _mqttClient.IsConnected;

        await PublishBirthMessage();
        OnConnectedAsync?.Invoke(EventArgs.Empty);
    }

    private async Task OnMqttDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
    {
        IsConnected = _mqttClient.IsConnected;

        if (arg.ClientWasConnected)
        {
            _logger.LogInformation("MQTT client disconnected.");
            await Task.Run(() => OnDisconnectedAsync?.Invoke(EventArgs.Empty));
        }
        else
        {
            _logger.LogWarning("Unable to make MQTT client connection.");
        }

        if (arg.Exception != null)
        {
            _logger.LogWarning("MQTT client disconnected with exception: {0}", arg.Exception.Message);
        }
    }

    private async Task PublishBirthMessage()
    {
        _logger.LogInformation(LoggingEvents.MqttConnection, "Publishing birth message.");
        await PublishGatewayState(MqttConstants.OnLineState);
    }

    private async Task PublishGatewayState(string state)
    {
        var payloadJson = JsonSerializer.Serialize(new AvailabilityStateMqttJson(state), MqttConstants.MqttJsonSerialiseOptions);
        await PublishAsync(MqttTopic.BuildGatewayStatusTopic(ClientMqttId), payloadJson);
    }

    private async Task PublishLastWillMessage()
    {
        _logger.LogInformation(LoggingEvents.MqttConnection, "Publishing last will and testament message.");
        await PublishGatewayState(MqttConstants.OffLineState);
    }
}