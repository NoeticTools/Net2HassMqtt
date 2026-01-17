using System.Text;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class SensorEntity(
    ISensorConfig config,
    string entityUniqueId,
    string deviceNodeId,
    INet2HassMqttClient mqttClient,
    IPropertyInfoReader propertyInfoReader,
    ILogger logger)
    : StateEntityBase<ISensorConfig>(config, entityUniqueId, deviceNodeId, mqttClient, propertyInfoReader, logger)
{
    protected override EntityConfigMqttJsonBase GetConfigurationMqttPayload(DeviceConfig deviceConfig)
    {
        return new SensorConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}