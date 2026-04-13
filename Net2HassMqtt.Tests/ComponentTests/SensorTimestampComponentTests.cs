using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.EntityMessages;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class SensorTimestampComponentTests : ComponentTestsBase
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
    public async Task DateTimeTimestampTest()
    {
        DeviceBuilder.SetupDateTimeTimestampSensor(Model);
        var initialValue = Model.DateTimeTimestamp;
        var increment = 2.Days();

        var result = await Run(() => Model.DateTimeTimestamp += increment, 2);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        var mqttMsgMatcher = new SensorEntityMqttMessages(ClientId, DeviceId, DeviceFriendlyName,
                                                          "timestamp_date_time",
                                                          "Timestamp (DateTime)",
                                                          SensorDeviceClass.Timestamp.HassDeviceClassName,
                                                          "");

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            mqttMsgMatcher.Config,
            mqttMsgMatcher.SendsState(initialValue + increment),
            mqttMsgMatcher.SendsState(initialValue + increment.Add(increment)),
            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task IntTimestampTest()
    {
        DeviceBuilder.SetupIntTimestampSensor(Model);

        var initialValue = Model.IntTimestamp;
        const int increment = 40000;
        var result = await Run(() => Model.IntTimestamp += increment, 2);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        var mqttMsgMatcher = new SensorEntityMqttMessages(ClientId, DeviceId, DeviceFriendlyName,
                                                          "timestamp_int",
                                                          "Timestamp (int)",
                                                          SensorDeviceClass.Timestamp.HassDeviceClassName,
                                                          "");

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            mqttMsgMatcher.Config,
            mqttMsgMatcher.SendsState(initialValue + increment),
            mqttMsgMatcher.SendsState(initialValue + 2 * increment),
            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}