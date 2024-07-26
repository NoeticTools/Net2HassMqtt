using System.ComponentModel;
using System.Dynamic;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


// ReSharper disable MemberCanBePrivate.Global

namespace NoeticTools.Net2HassMqtt.Entities;

/// <summary>
///     State entity base class.
/// </summary>
internal abstract class StateEntityBase<T> : IMqttPublisher, IMqttSubscriber
    where T : EntityConfigBase
{
    private readonly string _deviceNodeId;

    protected StateEntityBase(T config, string entityUniqueId, string deviceNodeId,
                              INet2HassMqttClient mqttClient, ILogger logger)
    {
        config.Validate();
        Config = config;
        Logger = logger;
        EntityUniqueId = entityUniqueId;
        _deviceNodeId = deviceNodeId;
        MqttClient = mqttClient;
        StatusPropertyReader = new StatusPropertyReader(config.Model!,
                                                        config.StatusPropertyName,
                                                        config.Domain.HassDomainName,
                                                        config.HassDeviceClassName,
                                                        config.UnitOfMeasurement!.HassUnitOfMeasurement,
                                                        logger);

        CommandHandler = new EntityCommandHandler(config.Model!,
                                                  config.CommandMethodName,
                                                  config.UnitOfMeasurement!.HassUnitOfMeasurement,
                                                  logger);
        CanCommand = CommandHandler.CanCommand;

        foreach (var configuration in config.Attributes) Attributes.Add(new EntityAttribute(configuration, logger));
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

    public IStatusPropertyReader StatusPropertyReader { get; }

    protected ILogger Logger { get; }

    /// <summary>
    ///     Can write received MQTT entity values to the model.
    ///     Always false for entity attributes.
    /// </summary>
    public bool CanCommand { get; }

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

    private async void OnEventActivated()
    {
        Console.WriteLine("OnEventActivated");
        var topic = new TopicBuilder().WithComponent(Config.MqttTopicComponent)
                                      .WithNodeId(_deviceNodeId)
                                      .WithObjectId(Config.EntityNodeId);
        dynamic payload = new ExpandoObject();
        payload.event_type = "press";
        await MqttClient.PublishStatusAsync(topic, payload);
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