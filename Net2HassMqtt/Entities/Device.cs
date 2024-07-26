using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class Device : IMqttSubscriber
{
    private readonly DeviceConfig _config;
    private readonly ILogger _logger;
    private readonly INet2HassMqttClient _mqttClient;
    private readonly Dictionary<string, IMqttEntity> _mqttEntitiesByEntityNodeId = new();
    private readonly Dictionary<string, IMqttSubscriber> _mqttSubscribersByEntityNodeId = new();

    public Device(DeviceConfig config, INet2HassMqttClient mqttClient, ILogger logger)
    {
        _config = config;
        _mqttClient = mqttClient;
        _logger = logger;
    }

    void IMqttSubscriber.OnReceived(ReceivedMqttMessage mqttMessage)
    {
        _logger.LogDebug(LoggingEvents.MqttReceived, "Device '{DeviceName}'.", _config.Name);

        var entityNodeId = mqttMessage.ObjectId;
        if (_mqttSubscribersByEntityNodeId.TryGetValue(entityNodeId, out var listener))
        {
            listener.OnReceived(mqttMessage);
        }
        else
        {
            _logger.LogWarning(LoggingEvents.MqttReceived, "Device '{DeviceName}' does not have any listeners. Message ignored", _config.Name);
        }
    }

    public async Task PublishConfigAsync()
    {
        foreach (var (_, entity) in _mqttEntitiesByEntityNodeId)
        {
            await entity.PublishConfigAsync(_config);
        }
    }

    public async Task PublishStateAsync()
    {
        foreach (var (_, entity) in _mqttEntitiesByEntityNodeId)
        {
            if (entity is IMqttPublisher publisher)
            {
                await publisher.PublishStateAsync();
            }
        }
    }

    public async Task StartAsync()
    {
        foreach (var entity in _mqttEntitiesByEntityNodeId.Values)
        {
            await entity.StartAsync();
        }

        await _mqttClient.SubscribeToSetCommandsAsync(_config.DeviceId.ToMqttTopicSnakeCase(), this);
    }

    public async Task StopAsync()
    {
        foreach (var entity in _mqttEntitiesByEntityNodeId.Values)
        {
            await entity.StopAsync();
        }
    }

    internal void AddEntity(string entityNodeId, IMqttEntity entity)
    {
        if (entity is IMqttPublisher { CanCommand: true })
        {
            AddMqttSubscriber((IMqttSubscriber)entity, entityNodeId);
        }

        _mqttEntitiesByEntityNodeId[entityNodeId] = entity;
    }

    private void AddMqttSubscriber(IMqttSubscriber listener, string entityNodeId)
    {
        _mqttSubscribersByEntityNodeId.Add(entityNodeId, listener);
    }
}