using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public interface IMessageMatcher
{
    string Match(List<MqttApplicationMessage> actualMessages);
}