using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class BinarySensorTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        base.BaseSetup();
        Client.Setup.ConnectsImmediately();
    }

    [TearDown]
    public void TearDown()
    {
        BaseTearDown();
    }

    [Test]
    public async Task DoorTest()
    {
        DeviceBuilder.SetupDoorIsOpenBinarySensor(Model);

        var result = await Run(() => Model.DoorIsOpen = !Model.DoorIsOpen, 4);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.SequenceWas(
        [
            MessageMatchers.BridgeState.Online,
            MessageMatchers.DoorIsOpenEntity.Config,

            MessageMatchers.DoorIsOpenEntity.On,
            MessageMatchers.DoorIsOpenEntity.Off,
            MessageMatchers.DoorIsOpenEntity.On,
            MessageMatchers.DoorIsOpenEntity.Off,

            MessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task BatteryChargingTest()
    {
        DeviceBuilder.SetupBatteryChargingBinarySensor(Model);

        var result = await Run(() => Model.BatteryCharging = !Model.BatteryCharging, 4);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMessages.Verify.SequenceWas(
        [
            MessageMatchers.BridgeState.Online,
            MessageMatchers.BatteryChargingEntity.Config,

            MessageMatchers.BatteryChargingEntity.Off,
            MessageMatchers.BatteryChargingEntity.On,
            MessageMatchers.BatteryChargingEntity.Off,
            MessageMatchers.BatteryChargingEntity.On,

            MessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}