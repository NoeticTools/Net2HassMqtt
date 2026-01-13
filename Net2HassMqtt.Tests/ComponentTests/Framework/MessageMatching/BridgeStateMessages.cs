namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public sealed class BridgeStateMessages
{
    public MessageMatcher Online =>
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "online"
            }
            """);

    public MessageMatcher Offline =>
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "offline"
            }
            """);
}