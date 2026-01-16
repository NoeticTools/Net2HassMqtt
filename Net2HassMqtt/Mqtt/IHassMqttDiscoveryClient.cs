using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;


namespace NoeticTools.Net2HassMqtt.Mqtt;

public interface IHassMqttDiscoveryClient
{
    /// <summary>
    ///     Home Assistant discovers and updates entities (and devices) when configuration published to the 'config' topic.
    /// </summary>
    Task PublishEntityConfigAsync<T>(string objectId, IEntityConfig config, DeviceConfig device, T payload)
        where T : EntityConfigMqttJsonBase;
}