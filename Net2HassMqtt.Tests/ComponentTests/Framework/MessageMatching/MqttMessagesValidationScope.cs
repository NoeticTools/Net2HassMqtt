using System.Text;
using MQTTnet;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public class MqttMessagesValidationScope(List<MqttApplicationMessage> messages)
{
    public MqttMessagesValidationScope ValidateNonePublished()
    {
        if (messages.Count > 0)
        {
            var errorMessage = new StringBuilder($"Expected no messages but {messages.Count} messages detected.\n");
            AppendDetectedMessages(errorMessage, messages);
            Assert.Fail(errorMessage.ToString());
        }
        return this;
    }

    public MqttMessagesValidationScope ValidateSequenceWas(List<IMessageMatcher> expectedMessageSequence)
    {
        var expectedQueue = new Queue<IMessageMatcher>(expectedMessageSequence);
        var actualMessages = new List<MqttApplicationMessage>(messages);
        var startingActualMessagesCount = messages.Count;
        var startingMatchersCount = expectedMessageSequence.Count;

        while (actualMessages.Count > 0)
        {
            if (expectedQueue.Count == 0)
            {
                var errorMessage = new StringBuilder($"Expected messages matchers required for last {actualMessages.Count} messages detected.\n");
                AppendDetectedMessages(errorMessage, actualMessages);
                Assert.Fail(errorMessage.ToString());
            }

            var expected = expectedQueue.Dequeue();
            if (!expected.Match(actualMessages))
            {
                Assert.Fail( 
                    $"""
                     Actual message {startingActualMessagesCount-actualMessages.Count+1} does not match expected matcher {startingMatchersCount-expectedQueue.Count}.
                     
                       Expected:
                     {expected}
                     
                       Was:
                     {MessageMatcher.ToString(actualMessages[0])}

                     """);
            }

            if (expectedQueue.Count > 0 && actualMessages.Count == 0)
            {
                var errorMessage = new StringBuilder($"{expectedQueue.Count} matchers left after all detected messages matched.\n");
                AppendDetectedMessages(errorMessage, actualMessages);
                Assert.Fail(errorMessage.ToString());
            }
        }

        return this;
    }

    private static void AppendDetectedMessages(StringBuilder message, List<MqttApplicationMessage> actualMessages)
    {
        message.Append("Detected messages:\n\n");
        foreach (var actual in actualMessages)
        {
            message.Append("  " + MessageMatcher.ToString(actual) + "\n\n");
        }
        message.AppendLine();
    }
}