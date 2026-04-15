using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class DateSensorComponentTests : ComponentTestsBase
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
    public async Task DateOnlyDateTest()
    {
        var initialValue = Model.DateOnlyDate;
        const int increment = 7;
        var data = new EntityMqttMessagesTestData<DateSensorUoM, DateOnly>("Date (DateOnly)",
                                                                                "date_date_only",
                                                                                DateSensorUoM.None,
                                                                                nameof(ComponentTestModel.DateOnlyDate),
                                                                                () => Model.DateOnlyDate =
                                                                                    Model.DateOnlyDate.AddDays(increment),
                                                                                stepCount => initialValue.AddDays(stepCount * increment));

        await Run(data);
    }

    private async Task Run<T>(EntityMqttMessagesTestData<DateSensorUoM, T> data)
    {
        DeviceBuilder.HasDateSensor(config =>
                                             config.OnModel(Model)
                                                   .WithStatusProperty(data.ModelPropertyName)
                                                   .WithFriendlyName(data.EntityFriendlyName)
                                                   .WithNodeId(data.NodeId)
                                                   .WithUnitOfMeasurement(data.UnitOfMeasurement));

        await RunMqttMessaging(data, SensorDeviceClass.Date.HassDeviceClassName);
    }
}