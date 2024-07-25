using System.ComponentModel;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


// ReSharper disable MemberCanBePrivate.Global

namespace NoeticTools.Net2HassMqtt.Entities;

internal abstract class EntityBase<T> : EntityPropertyBase, IMqttPublisher, IMqttSubscriber
    where T : EntityConfigBase
{
    private readonly string _deviceNodeId;

    protected EntityBase(T config, string entityUniqueId, string deviceNodeId,
                         INet2HassMqttClient mqttClient, ILogger logger)
        : base(config, logger)
    {
        config.Validate();
        Config = config;
        EntityUniqueId = entityUniqueId;
        _deviceNodeId = deviceNodeId;
        MqttClient = mqttClient;
        CommandHandler = new EntityCommandHandler(Model,
                                                  config.CommandMethodName,
                                                  config.UnitOfMeasurement!.HassUnitOfMeasurement,
                                                  logger);
        foreach (var configuration in config.Attributes)
        {
            Attributes.Add(new EntityAttribute(configuration, logger));
        }
    }

    protected string EntityUniqueId { get; }

    public EntityCommandHandler CommandHandler { get; set; }

    /// <summary>
    ///     The entity's configuration.
    /// </summary>
    protected T Config { get; }

    protected INet2HassMqttClient MqttClient { get; }

    /// <summary>
    ///     Attribute values to read and sent when MQTT publishing an entity value.
    /// </summary>
    private List<EntityAttribute> Attributes { get; } = [];

    /// <summary>
    ///     Can write received MQTT entity values to the model.
    ///     Always false for entity attributes.
    /// </summary>
    public bool CanCommand => CommandHandler.CanCommand;

    /// <summary>
    ///     Can read entity status from the model.
    /// </summary>
    public bool CanRead => StatusPropertyReader.CanRead;

    public async Task PublishConfigAsync(DeviceConfig deviceConfig)
    {
        var payloadJson = GetHasDiscoveryMqttPayload(deviceConfig);
        await MqttClient.Discovery.PublishEntityConfigAsync(EntityUniqueId, Config, deviceConfig, payloadJson);
    }

    public async Task PublishStateAsync()
    {
        var payload = new StateWithDataMqttJson(StatusPropertyReader.Read(), GetAttributeValuesDictionary());
        await PublishStatusAsync(payload);
    }

    public Task StartAsync()
    {
        Config.Model!.PropertyChanged += OnModelPropertyChanged;
        Config.SubscribeEvent?.Invoke(OnEventActivated);

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        Config.Model!.PropertyChanged -= OnModelPropertyChanged;
        Config.UnsubscribeEvent?.Invoke(OnEventActivated);
        return Task.CompletedTask;
    }

    void IMqttSubscriber.OnReceived(ReceivedMqttMessage message)
    {
        if (message.TopicAction == TopicAction.SetCmd)
        {
            CommandHandler.Handle(message.Payload);
        }
    }

    protected abstract EntityConfigMqttJsonBase GetHasDiscoveryMqttPayload(DeviceConfig deviceConfig);

    private Dictionary<string, string> GetAttributeValuesDictionary()
    {
        return Attributes.ToDictionary(attribute => attribute.Name, attribute => attribute.StatusPropertyReader.Read());
    }

    private async void OnEventActivated() {
        Console.WriteLine("OnEventActivated");
        var topic = new TopicBuilder().WithComponent(Config.MqttTopicComponent)
                                      .WithNodeId(_deviceNodeId)
                                      .WithObjectId(Config.EntityNodeId);
        var payload = new ActionWithDataMqttJson("press");
        await MqttClient.PublishCommandAsync(topic, payload);
    }

    private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Config.StatusPropertyName)
        {
            // ReSharper disable once UseDiscardAssignment
            var _ = PublishStateAsync();
        }
    }

    private async Task PublishStatusAsync<T2>(T2 status)
    {
        var topicBuilder = new TopicBuilder().WithComponent(Config.MqttTopicComponent)
                                             .WithNodeId(_deviceNodeId)
                                             .WithObjectId(Config.EntityNodeId);
        await MqttClient.PublishStatusAsync(topicBuilder, status);
    }
}