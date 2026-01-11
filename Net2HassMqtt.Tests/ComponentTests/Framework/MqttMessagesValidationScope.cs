using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

internal class MqttMessagesValidationScope(List<MqttApplicationMessage> messages)
{
    public MqttMessagesValidationScope Sequence(List<MqttMessageMatcher> expectedMessageSequence)
    {
        Assert.That(messages, Has.Count.EqualTo(expectedMessageSequence.Count), "The number of messages is different to number expected.");

        for (var index = 0; index < messages.Count; index++)
        {
            var actual = messages[index];
            var expected = expectedMessageSequence[index];

            Assert.That(expected.Matches(actual), Is.True,
                        $"""
                        Actual message {index+1} does not match the expected message.
                        
                          Expected:
                        {expected}
                        
                          Was:
                        {MqttMessageMatcher.ToString(actual)}
                        
                        """);
        }

        return this;
    }
}