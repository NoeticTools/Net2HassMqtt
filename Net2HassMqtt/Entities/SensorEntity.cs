using System.Text;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
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
        return Config.HassDeviceClassName != SensorDeviceClass.Enum.HassDeviceClassName ? new SensorConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId) : GetEnumSensorMqttConfig(deviceConfig);
    }

    private EntityConfigMqttJsonBase GetEnumSensorMqttConfig(DeviceConfig deviceConfig)
    {
        Config.UnitOfMeasurement = null; // see: https://www.home-assistant.io/integrations/sensor.mqtt/
        Config.Options ??= [];
        var statusValueType = PropertyInfoReader.GetPropertyGetterInfo(Config.Model!, Config.StatusPropertyName!)!.PropertyType;
        var enumNames = Enum.GetNames(statusValueType);
        foreach (var enumName in enumNames)
        {
            Config.Options.Add(enumName);
        }
        return new SensorConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}