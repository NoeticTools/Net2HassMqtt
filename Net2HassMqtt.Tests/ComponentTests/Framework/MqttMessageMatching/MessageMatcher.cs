using System.Text;
using System.Text.RegularExpressions;
using MQTTnet;
using MQTTnet.Protocol;
#pragma warning disable IDE0057


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;

public sealed class MessageMatcher(string topic, string expectedPayload) : IMessageMatcher
{
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

    private static (string fragment, int diffFromLeft) GetFragment(string payload, int firstDifferenceIndex, int charactersToLeft = 30, int charactersToRight = 40)
    {
        charactersToLeft = Math.Min(charactersToLeft, firstDifferenceIndex);
        charactersToRight = Math.Min(charactersToRight, payload.Length - firstDifferenceIndex);
        var fragment = payload.Substring(firstDifferenceIndex - charactersToLeft, charactersToLeft + charactersToRight).Replace("\r", @"\r").Replace("\n", @"\n");
        var diffFromLeft = charactersToLeft +fragment.Substring(0, charactersToLeft).Count(x => x == '\\');
        return (fragment, diffFromLeft);
    }

    /// <summary>
    ///     Compare actual and expected me
    /// </summary>
    /// <param name="actual"></param>
    /// <returns></returns>
    private string Matches(MqttApplicationMessage actual)
    {
        var actualPayload = Regex.Unescape(Encoding.UTF8.GetString(actual.PayloadSegment));
        var firstDifferenceIndex = FindFirstDifferenceIndex(actualPayload, expectedPayload);
        if (firstDifferenceIndex != -1)
        {
            var expected = GetFragment(expectedPayload, firstDifferenceIndex);
            var was = GetFragment(actualPayload, firstDifferenceIndex);
            var marker = "^".PadLeft(expected.diffFromLeft);
            return $"""
                    The payloads does not match what was expected.

                      Expected:  '...{expected.fragment}...'
                                     {marker}
                      But was:   '...{was.fragment}...'
                      
                    """;
        }

        var matches = actual.Topic.Equals(topic) &&
                      actual is
                      {
                          Retain: true,
                          Dup: false,
                          MessageExpiryInterval: 0,
                          PayloadFormatIndicator: MqttPayloadFormatIndicator.Unspecified,
                          QualityOfServiceLevel: MqttQualityOfServiceLevel.AtLeastOnce,
                          ContentType: null,
                          ResponseTopic: null
                      };

        return matches ? "" : "The message payload matched but other properties did not.";
    }
}