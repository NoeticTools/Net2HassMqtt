using FluentDate;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class DurationSensorComponentTests : ComponentTestsBase
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
    public async Task DoublePropertyWithDaysUoMTest()
    {
        var initialValue = Model.DoubleDuration;
        const double increment = 0.25;
        var data = new EntityMqttMessagesTestData<DurationSensorUoM, double>("Duration (Days)",
                                                                             "duration_days",
                                                                             DurationSensorUoM.Days,
                                                                             nameof(ComponentTestModel.DoubleDuration),
                                                                             () => Model.DoubleDuration += increment,
                                                                             stepCount => initialValue + increment * stepCount);

        await Run(data);
    }

    [Test]
    public async Task IntPropertyWithHoursUoMTest()
    {
        var initialValue = Model.IntDuration;
        const int increment = 7;
        var data = new EntityMqttMessagesTestData<DurationSensorUoM, int>("Duration (Hours)",
                                                                          "duration_hours",
                                                                          DurationSensorUoM.Hours,
                                                                          nameof(ComponentTestModel.IntDuration),
                                                                          () => Model.IntDuration += increment,
                                                                          stepCount => initialValue + increment * stepCount);

        await Run(data);
    }

    [Test]
    public async Task TimespanPropertyWithSecondsUoMTest()
    {
        var initialValue = Model.TimespanDuration;
        var increment = 3.Hours().TimeSpan;
        var data = new EntityMqttMessagesTestData<DurationSensorUoM, int>("Duration (Seconds)",
                                                                          "duration_seconds",
                                                                          DurationSensorUoM.Seconds,
                                                                          nameof(ComponentTestModel.TimespanDuration),
                                                                          () => Model.TimespanDuration += increment,
                                                                          stepCount => (int)initialValue
                                                                                            .Add(increment.Multiply(stepCount))
                                                                                            .TotalSeconds);

        await Run(data);
    }

    private async Task Run<T>(EntityMqttMessagesTestData<DurationSensorUoM, T> data)
    {
        DeviceBuilder.HasDurationSensor(config =>
                                            config.OnModel(Model)
                                                  .WithStatusProperty(data.ModelPropertyName)
                                                  .WithFriendlyName(data.EntityFriendlyName)
                                                  .WithNodeId(data.NodeId)
                                                  .WithUnitOfMeasurement(data.UnitOfMeasurement));

        await RunMqttMessaging(data, SensorDeviceClass.Duration.HassDeviceClassName);
    }
}