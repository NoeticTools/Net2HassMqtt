using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public interface IMessageMatcher
{
    bool Match(List<MqttApplicationMessage> actualMessages);
}