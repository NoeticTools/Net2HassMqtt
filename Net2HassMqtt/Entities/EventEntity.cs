using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;

namespace NoeticTools.Net2HassMqtt.Entities;

internal sealed class EventEntity : EventEntityBase<EventConfig>
{
    public EventEntity(EventConfig config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger) :
        base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
    }

    protected override EntityConfigMqttJsonBase GetHasDiscoveryMqttPayload(DeviceConfig deviceConfig)
    {
        if (!string.IsNullOrWhiteSpace(Config.EventTypeToSendAfterEachEvent))
        {
            if (!Config.EventTypes.Contains(Config.EventTypeToSendAfterEachEvent))
            {
                var list = Config.EventTypes.ToList();
                list.Add(Config.EventTypeToSendAfterEachEvent);
                Config.EventTypes = list.ToArray();
            }
        }
        var mqtt = new EventConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId)
        {
            ValueTemplate = "{{ value_json.event | tojson }}"
        };
        return mqtt;
    }
}