using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.UnitTests.Configuration;

[TestFixture]
public class EnumSensorConfigTests
{
    private EnumSensorConfig _target;

    [SetUp]
    public void SetUp()
    {
        var model = new ComponentTestModel
        {
            CurrentState = ComponentTestModel.TestStates.StateTwo
        };
        _target = new EnumSensorConfig(SensorDeviceClass.Enum)
        {
            Model = model,
            StatusPropertyName = nameof(ComponentTestModel.CurrentState)
        };
        ((IEntityConfig)_target).SetEntityId("entity_id");
        ((IEntityConfig)_target).EntityFriendlyName = "A friendly name";
    }

    [Test]
    public void AfterValidationUnitOfMeasurementIsNullTest()
    {
        _target.Validate();

        Assert.That(_target.UnitOfMeasurement, Is.Null,
                    "En enum sensor's MQTT unit_of_measurement must be null. See https://www.home-assistant.io/integrations/sensor.mqtt/");
        Assert.That(_target.Options, Has.Count.EqualTo(3));
        Assert.That(_target.Options[2], Is.EqualTo("StateThree"));
    }
}