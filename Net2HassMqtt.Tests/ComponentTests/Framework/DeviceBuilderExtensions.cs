using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration.Building;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

internal static class DeviceBuilderExtensions
{
    extension(DeviceBuilder deviceBuilder)
    {
        // todo - move these methods to their test classes. See SensorDurationComponentTests for example.

        public void SetupBatteryChargingBinarySensor(ComponentTestModel model)
        {
            deviceBuilder.HasBatteryChargingBinarySensor(config => config.OnModel(model)
                                                                         .WithStatusProperty(nameof(ComponentTestModel.BatteryCharging))
                                                                         .WithFriendlyName("Battery Charging Status")
                                                                         .WithNodeId("batt1_charging"));
        }

        public void SetupCurrentStateEnumSensor(ComponentTestModel model)
        {
            deviceBuilder.HasEnumSensor(config => config.OnModel(model)
                                                        .WithStatusProperty(nameof(ComponentTestModel.CurrentState))
                                                        .WithFriendlyName("Current State")
                                                        .WithNodeId("current_state"));
        }

        public void SetupDateTimeTimestampSensor(ComponentTestModel model)
        {
            deviceBuilder.HasTimestampSensor(config => config.OnModel(model)
                                                             .WithStatusProperty(nameof(ComponentTestModel.DateTimeTimestamp))
                                                             .WithFriendlyName("Timestamp (DateTime)")
                                                             .WithNodeId("timestamp_date_time"));
        }

        public void SetupDoorIsOpenBinarySensor(ComponentTestModel model)
        {
            deviceBuilder.HasDoorBinarySensor(config => config.OnModel(model)
                                                              .WithStatusProperty(nameof(ComponentTestModel.DoorIsOpen))
                                                              .WithFriendlyName("Door Open Status")
                                                              .WithNodeId("door_is_open"));
        }

        public void SetupIntTimestampSensor(ComponentTestModel model)
        {
            deviceBuilder.HasTimestampSensor(config => config.OnModel(model)
                                                             .WithStatusProperty(nameof(ComponentTestModel.IntTimestamp))
                                                             .WithFriendlyName("Timestamp (int)")
                                                             .WithNodeId("timestamp_int"));
        }
    }
}