using Microsoft.Extensions.Logging;
using Moq;
using Net2HassMqtt.Tests.UnitTests.Framework;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;
using NoeticTools.Net2HassMqtt.Framework;


namespace Net2HassMqtt.Tests.UnitTests.Entities.Framework.StatusProperty;

[TestFixture]
public class StatusPropertyReaderTests
{
    private const string PropertyName = "TestProperty";
    private Mock<ILogger> _logger;
    private TestModel _model;
    private Mock<IPropertyInfoReader> _propertyInfoReader;
    private PropertyInfoStub _propertyInfoStub;

    [SetUp]
    public void Setup()
    {
        _model = new TestModel();
        _propertyInfoReader = new Mock<IPropertyInfoReader>();
        _propertyInfoStub = new PropertyInfoStub(PropertyName, typeof(TimeSpan), true);
        _propertyInfoReader.Setup(x => x.GetPropertyGetterInfo(_model, PropertyName))
                           .Returns(_propertyInfoStub);
        _logger = new Mock<ILogger>();
    }

    [TestCase(0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyWithUoMDays(double days, string expected)
    {
        _propertyInfoStub.StubbedValue = TimeSpan.FromDays(days);
        RunTest(expected, HassUoMs.Days);
    }

    [TestCase(0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyWithUoMHours(double hours, string expected)
    {
        _propertyInfoStub.StubbedValue = TimeSpan.FromHours(hours);
        RunTest(expected, HassUoMs.Hours);
    }

    [TestCase(0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyWithUoMMinutes(double minutes, string expected)
    {
        _propertyInfoStub.StubbedValue = TimeSpan.FromMinutes(minutes);
        RunTest(expected, HassUoMs.Minutes);
    }

    [TestCase(0, "0")]
    [TestCase(0.123, "0.123")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyWithUoMSeconds(double seconds, string expected)
    {
        _propertyInfoStub.StubbedValue = TimeSpan.FromSeconds(seconds);
        var target = GetStatusPropertyReader(HassUoMs.Seconds);
        Assert.That(target.CanRead, Is.True);

        var result = target.Read();

        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(0, "0")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadIntegerPropertyWithUoMSeconds(int seconds, string expected)
    {
        _propertyInfoStub.StubbedValue = seconds;
        _propertyInfoStub.SetPropertyType(typeof(int));
        var target = GetStatusPropertyReader(HassUoMs.Seconds);
        Assert.That(target.CanRead, Is.True);

        var result = target.Read();

        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(0, "0")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadDoublePropertyWithUoMSeconds(double seconds, string expected)
    {
        _propertyInfoStub.StubbedValue = seconds;
        _propertyInfoStub.SetPropertyType(typeof(double));
        var target = GetStatusPropertyReader(HassUoMs.Seconds);
        Assert.That(target.CanRead, Is.True);

        var result = target.Read();

        Assert.That(result, Is.EqualTo(expected));
    }

    private StatusPropertyReader GetStatusPropertyReader(string hassUoM)
    {
        var target = new StatusPropertyReader(_model,
                                              PropertyName,
                                              HassDomains.Sensor.HassDomainName,
                                              SensorDeviceClass.Duration.HassDeviceClassName,
                                              hassUoM,
                                              _propertyInfoReader.Object,
                                              _logger.Object);
        return target;
    }

    private void RunTest(string expected, string hassUoM)
    {
        var target = GetStatusPropertyReader(hassUoM);
        Assert.That(target.CanRead, Is.True);

        var result = target.Read();

        Assert.That(result, Is.EqualTo(expected));
    }
}