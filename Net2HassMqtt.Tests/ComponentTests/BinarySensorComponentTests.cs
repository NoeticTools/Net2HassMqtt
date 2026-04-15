using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class BinarySensorComponentTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        BaseSetup();
        Client.Setup.ConnectsImmediately();
    }

    [TearDown]
    public void TearDown()
    {
        BaseTearDown();
    }

    [Test]
    public async Task BatteryChargingTest()
    {
        DeviceBuilder.HasBatteryChargingBinarySensor(config => config.OnModel(Model)
                                                                     .WithStatusProperty(nameof(ComponentTestModel.BatteryCharging))
                                                                     .WithFriendlyName("Battery Charging Status")
                                                                     .WithNodeId("batt1_charging"));

        var result = await Run(() => Model.BatteryCharging = !Model.BatteryCharging, 4);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            MqttMessageMatchers.BatteryChargingEntity.Config,

            MqttMessageMatchers.BatteryChargingEntity.Off,
            MqttMessageMatchers.BatteryChargingEntity.On,
            MqttMessageMatchers.BatteryChargingEntity.Off,
            MqttMessageMatchers.BatteryChargingEntity.On,

            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task DoorTest()
    {
        DeviceBuilder.HasDoorBinarySensor(config => config.OnModel(Model)
                                                          .WithStatusProperty(nameof(ComponentTestModel.DoorIsOpen))
                                                          .WithFriendlyName("Door Open Status")
                                                          .WithNodeId("door_is_open"));

        var result = await Run(() => Model.DoorIsOpen = !Model.DoorIsOpen, 4);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            MqttMessageMatchers.DoorIsOpenEntity.Config,

            MqttMessageMatchers.DoorIsOpenEntity.On,
            MqttMessageMatchers.DoorIsOpenEntity.Off,
            MqttMessageMatchers.DoorIsOpenEntity.On,
            MqttMessageMatchers.DoorIsOpenEntity.Off,

            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}