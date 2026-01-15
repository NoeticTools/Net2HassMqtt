using System.Text;
using MQTTnet;
using MQTTnet.Protocol;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;

public sealed class MessageMatcher(string topic, string expectedPayload) : IMessageMatcher
{
    /// <summary>
    /// Compare actual and expected me
    /// </summary>
    /// <param name="actual"></param>
    /// <returns></returns>
    private string Matches(MqttApplicationMessage actual)
    {
        var actualPayload = Encoding.UTF8.GetString(actual.PayloadSegment);
        var firstDifferenceIndex = FindFirstDifferenceIndex(actualPayload, expectedPayload);
        if (firstDifferenceIndex != -1)
        {
            var expectedFragment = GetFragment(expectedPayload, firstDifferenceIndex);
            var actualFragment = GetFragment(actualPayload, firstDifferenceIndex);
            return $"""
                    The payloads does not match what was expected.
                    
                      Expected:  '...{expectedFragment}...'
                      
                      But was:   '...{actualFragment}...'
                      
                    """;
        }

        var matches = actual.Topic.Equals(topic) &&
                      Encoding.UTF8.GetString(actual.PayloadSegment).Equals(expectedPayload) &&
                      actual is { 
                          Retain: true, 
                          Dup: false, 
                          MessageExpiryInterval: 0, 
                          PayloadFormatIndicator: MqttPayloadFormatIndicator.Unspecified, 
                          QualityOfServiceLevel: MqttQualityOfServiceLevel.AtLeastOnce, 
                          ContentType: null, 
                          ResponseTopic: null
                      };

        return matches ? "" : "oops";
    }

    private static string GetFragment(string payload, int firstDifferenceIndex, int charactersToLeft=50, int charactersToRight= 50)
    {
        charactersToLeft = Math.Min(charactersToLeft, firstDifferenceIndex);
        var fragmentLength = Math.Min(firstDifferenceIndex + 1 + charactersToRight, payload.Length-firstDifferenceIndex);
        return payload.Substring(firstDifferenceIndex-charactersToLeft, fragmentLength).Replace("\r", @"\r").Replace("\n", @"\n");
    }

    private static int FindFirstDifferenceIndex(string string1, string string2)
    {
        var minLength = Math.Min(string1.Length, string2.Length);
        for (var i = 0; i < minLength; i++)
        {
            if (string1[i] != string2[i])
            {
                return i;
            }
        }
    
        // If one string is a substring of the other, the difference is the length of the shorter one
        if (string1.Length != string2.Length)
        {
            return minLength;
        }

        return -1; // Strings are identical
    }

    public override string ToString()
    {
        return $"""
                   Topic:   {topic}
                   Payload: {expectedPayload}
                   Retain:  True
                   Dup:     False
                   MessageExpiryInterval:  0
                   PayloadFormatIndicator: Unspecified
                   QualityOfServiceLevel:  AtLeastOnce
                   ContentType:   null
                   ResponseTopic: null
               """;
    }

    public string Match(List<MqttApplicationMessage> actualMessages)
    {
        var actual = actualMessages[0];
        var result = Matches(actual);
        if (result.Length == 0)
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