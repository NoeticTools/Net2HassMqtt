using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public class AnyMessagesMatcher(int maximumNumberOfMessages) : IMessageMatcher
{
    public bool Match(List<MqttApplicationMessage> actualMessages)
    {
        var removeCount = Math.Min(maximumNumberOfMessages, actualMessages.Count);
        actualMessages.RemoveRange(0, removeCount);
        return true;
    }
}