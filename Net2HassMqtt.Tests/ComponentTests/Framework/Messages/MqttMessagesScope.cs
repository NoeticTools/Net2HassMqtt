using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Messages;

public class MqttMessagesScope(List<MqttApplicationMessage> messages)
{
    public MqttMessagesValidationScope Verify => new MqttMessagesValidationScope(messages);
}