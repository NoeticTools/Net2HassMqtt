using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

internal static class DeviceBuilderExtensions
{
    public static void SetupBatteryChargingBinarySensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasBatteryChargingBinarySensor(config => config.OnModel(model)
                                                                     .WithStatusProperty(nameof(ComponentTestModel.BatteryCharging))
                                                                     .WithFriendlyName("Battery Charging Status")
                                                                     .WithNodeId("batt1_charging"));
    }
    public static void SetupDoorIsOpenBinarySensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasDoorBinarySensor(config => config.OnModel(model)
                                                                     .WithStatusProperty(nameof(ComponentTestModel.DoorIsOpen))
                                                                     .WithFriendlyName("Door Open Status")
                                                                     .WithNodeId("door_is_open"));
    }

    public static void SetupEnumSensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasEnumSensor(config => config.OnModel(model)
                                                    .WithStatusProperty(nameof(ComponentTestModel.TestStates))
                                                    .WithFriendlyName("Current State")
                                                    .WithNodeId("current_state"));
    }
}
