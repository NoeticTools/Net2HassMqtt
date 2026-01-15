using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;

public interface IMessageMatcher
{
    string Match(List<MqttApplicationMessage> actualMessages);
}