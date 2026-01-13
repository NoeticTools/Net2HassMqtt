using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Messages;

public class MqttMessagesValidationScope(List<MqttApplicationMessage> messages)
{
    public MqttMessagesValidationScope NonePublished()
    {
        return SequenceWas([]);
    }

    public MqttMessagesValidationScope SequenceWas(List<MessageMatching> expectedMessageSequence)
    {
        var actualMessages = new List<MqttApplicationMessage>(messages);
        if (actualMessages.Count != expectedMessageSequence.Count)
        {
            var errorMessage = $"Expected {expectedMessageSequence.Count} messages but detected {actualMessages.Count} messages.";
            errorMessage += "\n";
            errorMessage += "Detected messages:\n\n";
            foreach (var actual in actualMessages)
            {
                errorMessage += "  " + MessageMatching.ToString(actual) + "\n\n";
            }
            errorMessage += "\n";
            Assert.Fail(errorMessage);
        }

        for (var index = 0; index < actualMessages.Count; index++)
        {
            var actual = actualMessages[index];
            var expected = expectedMessageSequence[index];

            Assert.That(expected.Matches(actual), Is.True,
                        $"""
                        Actual message {index+1} does not match the expected message.
                        
                          Expected:
                        {expected}
                        
                          Was:
                        {MessageMatching.ToString(actual)}
                        
                        """);
        }

        return this;
    }
}