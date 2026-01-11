using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.ComponentTests.Framework.ApplicationMessages;


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
        ManagedMqttClient.SetupIsConnected(false);

        var result = await Run();

        Client.Verify
              .WasStartedOnce()
              .NoSubscriptionsMade();

        PublishedMessages.Verify.NonePublished();

        Assert.That(result, Is.False, "Expected run to fail.");
    }

    [Test]
    public async Task BrokerSlowToConnectTest()
    {
        ManagedMqttClient.SetupIsConnected(false, false, true);

        var result = await Run();

        
        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.SequenceWas([MqttMessageMatcher.BridgeStateOfflineMessage]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task SendsBridgeOfflineWhenStoppedTest()
    {
        ManagedMqttClient.SetupIsConnected();

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