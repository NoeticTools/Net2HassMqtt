using System.Text;
using MQTTnet;
using MQTTnet.Protocol;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

internal class MqttMessageMatcher(string topic, string payload)
{
    public bool Matches(MqttApplicationMessage actual)
    {
        return actual.Topic.Equals(topic) &&
               Encoding.UTF8.GetString(actual.PayloadSegment).Equals(payload) &&
               actual is { 
                   Retain: true, 
                   Dup: false, 
                   MessageExpiryInterval: 0, 
                   PayloadFormatIndicator: MqttPayloadFormatIndicator.Unspecified, 
                   QualityOfServiceLevel: MqttQualityOfServiceLevel.AtLeastOnce, 
                   ContentType: null, 
                   ResponseTopic: null
               };
    }

    public override string ToString()
    {
        return $"""
                   Topic:   {topic}
                   Payload: {payload}
                   Retain:  True
                   Dup:     False
                   MessageExpiryInterval:  0
                   PayloadFormatIndicator: Unspecified
                   QualityOfServiceLevel:  AtLeastOnce
                   ContentType:   null
                   ResponseTopic: null
               """;
    }

    public static string ToString(MqttApplicationMessage message)
    {
        return $"""
                    Topic:   {message.Topic}
                    Payload: {Encoding.UTF8.GetString(message.PayloadSegment)}
                    Retain:  {message.Retain}
                    Dup:     {message.Dup}
                    MessageExpiryInterval:  {message.MessageExpiryInterval}
                    PayloadFormatIndicator: {message.PayloadFormatIndicator}
                    QualityOfServiceLevel:  {message.QualityOfServiceLevel} 
                    ContentType:   {message.ContentType ?? "null"}
                    ResponseTopic: {message.ResponseTopic ?? "null"}
                """;
    }
}