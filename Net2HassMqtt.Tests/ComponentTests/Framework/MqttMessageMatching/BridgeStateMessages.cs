namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;

public sealed class BridgeStateMessages
{
    public MessageMatcher Offline =>
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "offline"
            }
            """);

    public MessageMatcher Online =>
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "online"
            }
            """);
}