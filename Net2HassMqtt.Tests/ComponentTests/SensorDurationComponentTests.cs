using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.EntityMessages;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class SensorDurationComponentTests : ComponentTestsBase
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
    public async Task HoursTest()
    {
        var initialValue = Model.IntDuration;
        const int increment = 7;
        var loopAction = new Action(() => Model.IntDuration += increment);
        var getExpectedValue = new Func<int, int>(stepCount => initialValue + stepCount * increment);

        await RunMqttMessaging(nameof(ComponentTestModel.IntDuration),
                               "Duration (Hours)",
                               "duration_hours",
                               SensorDeviceClass.Duration.HassDeviceClassName,
                               DurationSensorUoM.Hours,
                               loopAction,
                               getExpectedValue);
    }

    [Test]
    public async Task SecondsTest()
    {
        var initialValue = Model.TimespanDuration;
        var increment = 3.Hours();
        var loopAction = new Action(() => Model.TimespanDuration += increment);
        var getExpectedValue = new Func<int, int>(stepCount =>
                                                      (int)initialValue.Add(increment.TimeSpan.Multiply(stepCount)).TotalSeconds);

        await RunMqttMessaging(nameof(ComponentTestModel.TimespanDuration),
                               "Duration (Seconds)",
                               "duration_seconds",
                               SensorDeviceClass.Duration.HassDeviceClassName,
                               DurationSensorUoM.Seconds,
                               loopAction,
                               getExpectedValue);
    }

    private async Task RunMqttMessaging(string modelPropertyName,
                                        string entityFriendlyName,
                                        string nodeId,
                                        string deviceClassName,
                                        DurationSensorUoM unitOfMeasurement,
                                        Action loopAction,
                                        Func<int, int> getExpectedValue)
    {
        DeviceBuilder.HasDurationSensor(config => config.OnModel(Model)
                                                        .WithStatusProperty(modelPropertyName)
                                                        .WithFriendlyName(entityFriendlyName)
                                                        .WithNodeId(nodeId)
                                                        .WithUnitOfMeasurement(unitOfMeasurement));


        var result = await Run(loopAction, 2);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        var mqttMsgMatcher = new SensorEntityMqttMessages(ClientId, DeviceId, DeviceFriendlyName,
                                                          nodeId,
                                                          entityFriendlyName,
                                                          deviceClassName,
                                                          unitOfMeasurement.HassUnitOfMeasurement);

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            mqttMsgMatcher.Config,
            mqttMsgMatcher.SendsState(getExpectedValue(1)),
            mqttMsgMatcher.SendsState(getExpectedValue(2)),
            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}