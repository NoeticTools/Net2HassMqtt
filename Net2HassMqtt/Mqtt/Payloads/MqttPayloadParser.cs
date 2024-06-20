using System.Text;
using MQTTnet;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads;

internal sealed class MqttPayloadParser
{
    /// <summary>
    ///     Parse a received MQTT payload.
    /// </summary>
    public static ReceivedMqttMessage Parse(MqttApplicationMessage message)
    {
        var payload = Encoding.UTF8.GetString(message.PayloadSegment);
        return new ReceivedMqttMessage(TopicBuilder.Parse(message.Topic), payload);
    }
}