namespace Net2HassMqtt.Tests.ComponentTests.Framework.Messages;

public sealed class BridgeStateMessages
{
    public MessageMatching Online =>
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "online"
            }
            """);

    public MessageMatching Offline =>
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "offline"
            }
            """);
}