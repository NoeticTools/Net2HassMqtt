using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.Building;


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

    public static void SetupIntTimestampSensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasTimestampSensor(config => config.OnModel(model)
                                                         .WithStatusProperty(nameof(ComponentTestModel.IntTimestamp))
                                                         .WithFriendlyName("Timestamp (int)")
                                                         .WithNodeId("timestamp_int"));
    }

    public static void SetupDateTimeTimestampSensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasTimestampSensor(config => config.OnModel(model)
                                                    .WithStatusProperty(nameof(ComponentTestModel.DateTimeTimestamp))
                                                    .WithFriendlyName("Timestamp (DateTime)")
                                                    .WithNodeId("timestamp_date_time"));
    }

    public static void SetupCurrentStateEnumSensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasEnumSensor(config => config.OnModel(model)
                                                    .WithStatusProperty(nameof(ComponentTestModel.CurrentState))
                                                    .WithFriendlyName("Current State")
                                                    .WithNodeId("current_state"));
    }

    public static void SetupDoorIsOpenBinarySensor(this DeviceBuilder deviceBuilder, ComponentTestModel model)
    {
        deviceBuilder.HasDoorBinarySensor(config => config.OnModel(model)
                                                          .WithStatusProperty(nameof(ComponentTestModel.DoorIsOpen))
                                                          .WithFriendlyName("Door Open Status")
                                                          .WithNodeId("door_is_open"));
    }
}