using System.Text;
using MQTTnet;
using MQTTnet.Protocol;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public sealed class MessageMatcher(string topic, string payload) : IMessageMatcher
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

    public bool Match(List<MqttApplicationMessage> actualMessages)
    {
        var actual = actualMessages[0];
        var result = Matches(actual);
        if (result)
        {
            actualMessages.RemoveAt(0);
        }
        return result;
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