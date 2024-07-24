using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;

namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class ButtonEntity : EntityBase<ButtonConfig>
{
    public ButtonEntity(ButtonConfig config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger) :
        base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
    }

    protected override EntityConfigMqttJsonBase GetHasDiscoveryMqttPayload(DeviceConfig deviceConfig)
    {
        return new ButtonConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}