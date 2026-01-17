using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;

public class AnyMessagesMatcher(int maximumNumberOfMessages) : IMessageMatcher
{
    public string Match(List<MqttApplicationMessage> actualMessages)
    {
        if (actualMessages.Count == 0)
        {
            return "Expected any message but no message detected. Must be one or more messages.";
        }
        var removeCount = Math.Min(maximumNumberOfMessages, actualMessages.Count);
        actualMessages.RemoveRange(0, removeCount);
        return "";
    }
}