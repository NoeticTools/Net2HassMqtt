using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.TestProperties;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Entities;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class TimestampSensorComponentTests : ComponentTestsBase
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
    public async Task IntTimestampTest()
    {
        DeviceBuilder.SetupIntTimestampSensor(Model);

        var initialValue = Model.IntTimestamp;
        const int increment = 40000;
        var result = await Run(() => Model.IntTimestamp += increment, 2);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        var mqttMsgMatcher = new TimestampEntityMqttMessages(ClientId, DeviceId, DeviceFriendlyName,
                                                          "timestamp_int",
                                                          "Timestamp (int)",
                                                          "sensor",
                                                          "timestamp");

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            mqttMsgMatcher.Config,
            mqttMsgMatcher.SendsState(initialValue + increment),
            mqttMsgMatcher.SendsState(initialValue + 2*increment),
            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
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

        var mqttMsgMatcher = new TimestampEntityMqttMessages(ClientId, DeviceId, DeviceFriendlyName,
                                                             "timestamp_date_time",
                                                             "Timestamp (DateTime)",
                                                             "sensor",
                                                             "timestamp");

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
}