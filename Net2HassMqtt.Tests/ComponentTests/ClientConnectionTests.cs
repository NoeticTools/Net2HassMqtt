using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.ComponentTests.Framework.ApplicationMessages;
using Net2HassMqtt.Tests.ComponentTests.Framework.Client;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class ClientConnectionTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        base.BaseSetup();
        DeviceBuilder.SetupBatteryChargingBinarySensor(Model);
    }

    [Test]
    public async Task BrokerFailsToConnectTest()
    {
        Client.Setup.IsConnected(false);

        var result = await Run();

        Client.Verify
              .WasStartedOnce()
              .NoSubscriptionsMade();

        PublishedMessages.Verify
                         .NonePublished();

        Assert.That(result, Is.False, "Expected run to fail.");
    }

    [Test]
    public async Task BrokerSlowToConnectTest()
    {
        Client.Setup.IsConnected(false, false, true);

        var result = await Run();

        
        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify
                         .SequenceWas([MqttMessageMatcher.BridgeStateOfflineMessage]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task SendsBridgeOfflineWhenStoppedTest()
    {
        Client.Setup.IsConnected();

        var result = await Run(ToggleChargingStatus, 5);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.SequenceWas([
            MqttMessageMatcher.BatteryChargingStateOffMessage,
            MqttMessageMatcher.BatteryChargingStateOnMessage,
            MqttMessageMatcher.BatteryChargingStateOffMessage,
            MqttMessageMatcher.BatteryChargingStateOnMessage,
            MqttMessageMatcher.BatteryChargingStateOffMessage,
            MqttMessageMatcher.BridgeStateOfflineMessage,
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}