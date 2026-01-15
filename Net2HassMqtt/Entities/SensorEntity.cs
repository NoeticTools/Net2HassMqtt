using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using System.Reflection;
using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class SensorEntity(
    SensorConfig config,
    string entityUniqueId,
    string deviceNodeId,
    INet2HassMqttClient mqttClient,
    IPropertyInfoReader propertyInfoReader,
    ILogger logger)
    : StateEntityBase<SensorConfig>(config, entityUniqueId, deviceNodeId, mqttClient, propertyInfoReader, logger)
{
    protected override EntityConfigMqttJsonBase GetConfigurationMqttPayload(DeviceConfig deviceConfig)
    {
        var configMqttJson = new SensorConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
        if (Config.HassDeviceClassName != SensorDeviceClass.Enum.HassDeviceClassName)
        {
            return configMqttJson;
        }

        var statusValueType = propertyInfoReader.GetPropertyGetterInfo(Config.Model!, Config.StatusPropertyName!)!.PropertyType;
        var enumNames = Enum.GetNames(statusValueType);
        configMqttJson.Options = string.Join(",", enumNames);
        return configMqttJson;
    }
}