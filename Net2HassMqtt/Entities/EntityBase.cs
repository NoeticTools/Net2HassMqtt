using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Entities;

internal abstract class EntityBase<T> : IMqttEntity
    where T : EntityConfigBase
{
    private readonly List<EntityAttribute> _attributes = [];

    protected EntityBase(T config, string entityUniqueId, string deviceNodeId,
                         INet2HassMqttClient mqttClient, ILogger logger)
    {
        config.Validate();
        Config = config;
        Logger = logger;
        EntityUniqueId = entityUniqueId;
        DeviceNodeId = deviceNodeId;
        MqttClient = mqttClient;
        foreach (var configuration in config.Attributes)
        {
            _attributes.Add(new EntityAttribute(configuration, logger));
        }
    }

    protected async Task PublishStatusAsync<TP>(TP payload)
    {
        var topic = new TopicBuilder().WithComponent(Config.MqttTopicComponent)
                                      .WithNodeId(DeviceNodeId)
                                      .WithObjectId(Config.EntityNodeId);
        await MqttClient.PublishStatusAsync(topic, payload);
    }

    /// <summary>
    ///     Can write received MQTT entity values to the model.
    ///     Always false for entity attributes.
    /// </summary>
    public bool CanCommand { get; protected init; }

    /// <summary>
    ///     The entity's configuration.
    /// </summary>
    protected T Config { get; }

    protected string DeviceNodeId { get; }

    protected string EntityUniqueId { get; }

    protected ILogger Logger { get; }

    protected INet2HassMqttClient MqttClient { get; }

    public async Task PublishConfigAsync(DeviceConfig deviceConfig)
    {
        var payloadJson = GetHasDiscoveryMqttPayload(deviceConfig);
        await MqttClient.Discovery.PublishEntityConfigAsync(EntityUniqueId, Config, deviceConfig, payloadJson);
    }

    public virtual Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task StopAsync()
    {
        return Task.CompletedTask;
    }

    protected Dictionary<string, string> GetAttributeValuesDictionary()
    {
        return _attributes.ToDictionary(attribute => attribute.Name, attribute => attribute.StatusPropertyReader.Read());
    }

    protected abstract EntityConfigMqttJsonBase GetHasDiscoveryMqttPayload(DeviceConfig deviceConfig);
}