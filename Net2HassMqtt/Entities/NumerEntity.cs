using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;


namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class NumberEntity : StateEntityBase<NumberConfig>
{
    public NumberEntity(NumberConfig config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger) :
        base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
    }

    protected override EntityConfigMqttJsonBase GetHasDiscoveryMqttPayload(DeviceConfig deviceConfig)
    {
        return new NumberConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}