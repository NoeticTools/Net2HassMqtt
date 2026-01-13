using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.ComponentTests.Framework.Messages;


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

    [TearDown]
    public void TearDown()
    {
        Task.Delay(10.Milliseconds()).Wait();   // todo: code smell, required to avoid intermittency in number of messages received
                                                // fix in separate issue
    }

    [Test]
    public async Task BrokerFailsToConnectTest()
    {
        Client.Setup.NeverConnects();

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
        Client.Setup.ConnectsAfterDelay();

        var result = await Run();
        
        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify
                         .SequenceWas([
                             MessageMatchers.BridgeState.Online,
                             MessageMatchers.BatteryChargingEntity.Config,
                             MessageMatchers.BridgeState.Offline
                         ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task SendsBridgeOfflineWhenStoppedTest()
    {
        Client.Setup.ConnectsImmediately();

        var result = await Run(ToggleChargingStatus, 5);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.SequenceWas([
            //MessageMatchers.BatteryChargingEntity.On, // todo: this looks odd (intermittent)

            MessageMatchers.BridgeState.Online,
            MessageMatchers.BatteryChargingEntity.Config,

            MessageMatchers.BatteryChargingEntity.Off,
            MessageMatchers.BatteryChargingEntity.On,
            MessageMatchers.BatteryChargingEntity.Off,
            MessageMatchers.BatteryChargingEntity.On,
            MessageMatchers.BatteryChargingEntity.Off,

            MessageMatchers.BridgeState.Offline,

            MessageMatchers.BatteryChargingEntity.Off, // todo: this looks odd
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}