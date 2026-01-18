using Microsoft.Extensions.Logging;
using Moq;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Entities;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Net2HassMqtt.Tests.UnitTests.Entities;

[TestFixture]
public class SensorEntityTests
{
    [Test]
    public void EnumSensorCanCommandIsFalseTest()
    {
        var model = new ComponentTestModel
        {
            CurrentState = ComponentTestModel.TestStates.StateTwo
        };
        const string statusPropertyName = nameof(ComponentTestModel.CurrentState);

        var sensorConfig = new Mock<ISensorConfig>();
        sensorConfig.SetupGet(x => x.Model).Returns(model);
        sensorConfig.SetupGet(x => x.StatusPropertyName).Returns(statusPropertyName);
        sensorConfig.SetupGet(x => x.Attributes).Returns([]);
        sensorConfig.SetupGet(x => x.Domain).Returns(new HassDomain("hass_domain_name", "domain_name"));
        sensorConfig.SetupGet(x => x.UnitOfMeasurement).Returns(EnumSensorUoM.None);
        sensorConfig.SetupGet(x => x.HassDeviceClassName).Returns("enum");

        var client = new Mock<INet2HassMqttClient>();
        var propertyInfoReader = new Mock<IPropertyInfoReader>();
        propertyInfoReader.Setup(x => x.GetPropertyGetterInfo(model, statusPropertyName))
                          .Returns(model.GetType().GetProperty(statusPropertyName, BindingFlags.Instance | BindingFlags.Public));
        var logger = new Mock<ILogger>();

        var entity = new SensorEntity(sensorConfig.Object, 
                                      "entity_id", 
                                      "node_id", 
                                      client.Object, 
                                      propertyInfoReader.Object, 
                                      logger.Object);

        Assert.IsFalse(entity.CanCommand);
    }
}
