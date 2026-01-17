using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;


namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class NumberEntity(
    NumberConfig config,
    string entityUniqueId,
    string deviceNodeId,
    INet2HassMqttClient mqttClient,
    IPropertyInfoReader propertyInfoReader,
    ILogger logger)
    : StateEntityBase<NumberConfig>(config, entityUniqueId, deviceNodeId, mqttClient, propertyInfoReader, logger)
{
    protected override EntityConfigMqttJsonBase GetConfigurationMqttPayload(DeviceConfig deviceConfig)
    {
        return new NumberConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}