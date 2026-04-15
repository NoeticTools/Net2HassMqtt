using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


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
    public async Task DateTimeTimestampTest()
    {
        var initialValue = Model.DateTimeTimestamp;
        const int increment = 2;

        var data = new EntityMqttMessagesTestData<TimestampSensorUoM, DateTime>("Timestamp (DateTime)",
                                                                                "timestamp_date_time",
                                                                                TimestampSensorUoM.None,
                                                                                nameof(ComponentTestModel.DateTimeTimestamp),
                                                                                () => Model.DateTimeTimestamp += TimeSpan.FromDays(increment),
                                                                                stepCount => initialValue.AddDays(stepCount * increment));

        await Run(data);
    }

    [Test]
    public async Task IntTimestampTest()
    {
        // todo - remove support for int timestamp in next major revision - not a good fit for a timestamp sensor, but leaving in for now to support existing users who may be using it with unix timestamps
        var initialValue = Model.IntTimestamp;
        const int increment = 40000;

        var data = new EntityMqttMessagesTestData<TimestampSensorUoM, int>("Timestamp (int)",
                                                                           "timestamp_int",
                                                                           TimestampSensorUoM.None,
                                                                           nameof(ComponentTestModel.IntTimestamp),
                                                                           () => Model.IntTimestamp += increment,
                                                                           stepCount => initialValue + stepCount * increment);

        await Run(data);
    }

    private async Task Run<T>(EntityMqttMessagesTestData<TimestampSensorUoM, T> data)
    {
        DeviceBuilder.HasTimestampSensor(config =>
                                             config.OnModel(Model)
                                                   .WithStatusProperty(data.ModelPropertyName)
                                                   .WithFriendlyName(data.EntityFriendlyName)
                                                   .WithNodeId(data.NodeId)
                                                   .WithUnitOfMeasurement(data.UnitOfMeasurement));

        await RunMqttMessaging(data, SensorDeviceClass.Timestamp.HassDeviceClassName);
    }
}