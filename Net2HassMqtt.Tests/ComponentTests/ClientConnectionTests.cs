using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class ClientConnectionTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        BaseSetup();
        DeviceBuilder.SetupBatteryChargingBinarySensor(Model);
    }

    [TearDown]
    public void TearDown()
    {
        BaseTearDown();
    }

    [Test]
    public async Task BrokerFailsToConnectTest()
    {
        Client.Setup.NeverConnects();

        var result = await Run();

        Client.Verify
              .WasStartedOnce()
              .NoSubscriptionsMade();

        PublishedMqttMessages.Verify
                         .ValidateNonePublished();

        Assert.That(result, Is.False, "Expected run to fail.");
    }

    [Test]
    public async Task BrokerSlowToConnectTest()
    {
        Client.Setup.ConnectsAfterDelay();

        var result = await Run();
        
        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMqttMessages.Verify
                         .MatchSequence([
                             MqttMessageMatchers.BridgeState.Online,
                             MqttMessageMatchers.BatteryChargingEntity.Config,
                             MqttMessageMatchers.Any() // todo - timing hack
                         ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task SendsBridgeOfflineWhenStoppedTest()
    {
        Client.Setup.ConnectsImmediately();

        var result = await Run(() => Model.BatteryCharging = !Model.BatteryCharging, 3);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMqttMessages.Verify.MatchSequence([
            //MqttMessageMatchers.BatteryChargingEntity.On, // todo: this looks odd (intermittent) - work on this separately

            MqttMessageMatchers.BridgeState.Online,
            MqttMessageMatchers.BatteryChargingEntity.Config,

            MqttMessageMatchers.BatteryChargingEntity.Off,
            MqttMessageMatchers.BatteryChargingEntity.On,
            MqttMessageMatchers.BatteryChargingEntity.Off,

            MqttMessageMatchers.BridgeState.Offline,

            MqttMessageMatchers.Any()
            //MqttMessageMatchers.BatteryChargingEntity.Off, // todo: this looks odd
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}