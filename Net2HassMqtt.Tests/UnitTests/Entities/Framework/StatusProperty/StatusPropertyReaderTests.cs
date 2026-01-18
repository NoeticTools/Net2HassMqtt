using System;
using System.Collections.Generic;
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
    private Mock<IPropertyInfoReader> _propertyInfoReader;
    private Mock<ILogger> _logger;
    private TestModel _model;
    private PropertyInfoStub _propertyInfoStub;
    private const string TimeSpanPropertyName = "TestProperty";

    [SetUp]
    public void Setup()
    {
        _model = new TestModel();
        _propertyInfoReader = new Mock<IPropertyInfoReader>();
        _propertyInfoStub = new PropertyInfoStub(TimeSpanPropertyName, typeof(TimeSpan), true);
        _propertyInfoReader.Setup(x => x.GetPropertyGetterInfo(_model, TimeSpanPropertyName))
                           .Returns(_propertyInfoStub);
        _logger = new Mock<ILogger>();
    }

    [TestCase(0, "0")]
    [TestCase(0.123, "0.123")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyInSeconds(double seconds, string expected)
    {
        _propertyInfoStub.StubedValue = TimeSpan.FromSeconds(seconds);
        var target = GetStatusPropertyReader(HassUoMs.Seconds);
        Assert.That(target.CanRead, Is.True);

        var result = target.Read();

        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyInMinutes(double minutes, string expected)
    {
        _propertyInfoStub.StubedValue = TimeSpan.FromMinutes(minutes);
        RunTest(expected, HassUoMs.Minutes);
    }

    [TestCase(0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyInHours(double hours, string expected)
    {
        _propertyInfoStub.StubedValue = TimeSpan.FromHours(hours);
        RunTest(expected, HassUoMs.Hours);
    }

    [TestCase(0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7, "7")]
    [TestCase(5000, "5000")]
    [TestCase(-7, "-7")]
    public void CanReadTimeSpanPropertyInDays(double days, string expected)
    {
        _propertyInfoStub.StubedValue = TimeSpan.FromDays(days);
        RunTest(expected, HassUoMs.Days);
    }

    private void RunTest(string expected, string hassUoM)
    {
        var target = GetStatusPropertyReader(hassUoM);
        Assert.That(target.CanRead, Is.True);

        var result = target.Read();

        Assert.That(result, Is.EqualTo(expected));
    }

    private StatusPropertyReader GetStatusPropertyReader(string hassUoM)
    {
        var target = new StatusPropertyReader(_model, 
                                              TimeSpanPropertyName,
                                              HassDomains.Sensor.HassDomainName,
                                              SensorDeviceClass.Duration.HassDeviceClassName,
                                              hassUoM,
                                              _propertyInfoReader.Object,
                                              _logger.Object);
        return target;
    }
}