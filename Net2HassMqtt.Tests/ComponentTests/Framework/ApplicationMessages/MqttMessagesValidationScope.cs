using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.ApplicationMessages;

public class MqttMessagesValidationScope(List<MqttApplicationMessage> messages)
{
    public MqttMessagesValidationScope NonePublished()
    {
        return SequenceWas([]);
    }

    public MqttMessagesValidationScope SequenceWas(List<MessageMatcher> expectedMessageSequence)
    {
        if (messages.Count != expectedMessageSequence.Count)
        {
            var errorMessage = $"Expected {expectedMessageSequence.Count} messages but detected {messages.Count} messages.";
            errorMessage += "\n";
            errorMessage += "Detected messages:\n";
            foreach (var actual in messages)
            {
                errorMessage += "  " + MessageMatcher.ToString(actual) + "\n\n";
            }
            errorMessage += "\n";
            Assert.Fail(errorMessage);
        }

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
                        {MessageMatcher.ToString(actual)}
                        
                        """);
        }

        return this;
    }
}