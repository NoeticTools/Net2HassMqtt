using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public class MqttMessagesScope(List<MqttApplicationMessage> messages)
{
    public MqttMessagesValidationScope Verify => new MqttMessagesValidationScope(messages);
}