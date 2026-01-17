using System.Text.Json;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Mqtt;

/// <summary>
///     MQTT publish methods that allow Home Assistant to automatically discover entities and devices.
/// </summary>
/// <remarks>
///     <para>See also:</para>
///     <list type="bullet">
///         <item>
///             <a href="https://home-assistant-china.github.io/docs/mqtt/discovery/">Home Assistant MQTT discovery</a>
///         </item>
///         <item>
///             <a href="https://www.home-assistant.io/integrations/switch.mqtt/">Home Assistant Switch MQTT</a>
///         </item>
///         <item>
///             <a href="https://www.home-assistant.io/integrations/sensor.mqtt/">Home Assistant Sensor MQTT</a>
///         </item>
///     </list>
/// </remarks>
internal sealed class HassDiscoveryClient(string mqttClientId, INet2HassMqttClient mqttClient) : IHassMqttDiscoveryClient
{
    /// <summary>
    ///     Home Assistant (HASS) discovers and updates entities (and devices) when configuration published to the 'config'
    ///     topic.
    /// </summary>
    public async Task PublishEntityConfigAsync<T>(string objectId, IEntityConfig config, DeviceConfig device, T payload)
        where T : EntityConfigMqttJsonBase
    {
        payload.ObjectId = objectId;

        var topic = new TopicBuilder().WithBaseTopic(mqttClientId)
                                      .WithComponent(payload.MqttTopicComponent)
                                      .WithNodeId(device.DeviceId)
                                      .WithObjectId(config.EntityNodeId);
        payload.Build(topic);

        var discoveryTopic = new TopicBuilder().WithBaseTopic(device.HomeAssistantClientId)
                                               .WithComponent(payload.MqttTopicComponent)
                                               .WithNodeId(device.DeviceId)
                                               .WithObjectId(payload.ObjectId)
                                               .WithAction(TopicAction.Config)
                                               .BuildHassDiscoveryTopic();

        var payloadJson = JsonSerializer.Serialize(payload, payload!.GetType(), MqttConstants.MqttJsonSerialiseOptions);
        await mqttClient.PublishAsync(discoveryTopic, payloadJson);
    }
}