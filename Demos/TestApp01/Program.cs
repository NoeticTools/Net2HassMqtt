using FluentDate;
using Microsoft.Extensions.Configuration;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.TestApp01.Models;


// ReSharper disable AccessToDisposedClosure

namespace NoeticTools.TestApp01;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Net2HassMqtt TestApp01.");
        Console.WriteLine();

        var appConfig = new ConfigurationBuilder()
                        .AddUserSecrets<Program>()
                        .Build();

        var binarySensorTestModel = new BinarySensorTestModel();
        var sensorTestModel = new SensorTestModel();
        var switchTestModel = new SwitchTestModel();

        var device1 = ConfigureDevice1(binarySensorTestModel);
        var device2 = ConfigureDevice2(sensorTestModel);
        var device3 = ConfigureDevice3(sensorTestModel);
        var device4 = ConfigureDevice4(switchTestModel);
        //var device5 = ConfigureDevice5(sensorTestModel); // humidifier
        var device6 = ConfigureDevice6(binarySensorTestModel);

        // todo cover, humidifier, & number ?

        var mqttClientOptions = HassMqttClientFactory.CreateQuickStartOptions("net2hassmqtt_test_app_1", appConfig);
        var bridge = new BridgeConfiguration()
                     .WithMqttOptions(mqttClientOptions)
                     .HasDevice(device1)
                     .HasDevice(device2)
                     .HasDevice(device3)
                     .HasDevice(device4)
                     //.HasDevice(device5)
                     .HasDevice(device6)
                     .Build();

        try
        {
            // Start the bridge
            await bridge.StartAsync();

            Console.WriteLine();
            Console.WriteLine("Press x to exit.");

            while (true)
            {
                await Task.Delay(100.Milliseconds());
                if (!Console.KeyAvailable)
                {
                    continue;
                }

                var key = Console.ReadKey();
                if (key.KeyChar == 'x')
                {
                    break;
                }

                if (key.KeyChar == '1')
                {
                    binarySensorTestModel.Status1 = !binarySensorTestModel.Status1;
                }
            }

            await bridge.StopAsync();
        }
        finally
        {
            await bridge.StopAsync();
            await Task.Delay(100.Milliseconds());
        }
    }

    private static DeviceBuilder ConfigureDevice6(BinarySensorTestModel model)
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Cover Test Device")
                                        .WithId("net2hassmqtt_test_device_06");

        device.HasAwningCover(config => config.OnModel(model)
                                              .WithStatusProperty(nameof(BinarySensorTestModel.Status1))
                                              .WithFriendlyName("Awning")
                                              .WithNodeId("status_1"))
              .HasBlindCover(config => config.OnModel(model)
                                             .WithStatusProperty(nameof(BinarySensorTestModel.Status2))
                                             .WithFriendlyName("Blind")
                                             .WithNodeId("status_2"));
        return device;
    }

    private static DeviceBuilder ConfigureDevice5(SensorTestModel model)
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Humidifier Test Device")
                                        .WithId("net2hassmqtt_test_device_05");

        device.HasHumidifier(config => config.OnModel(model)
                                             .WithStatusProperty(nameof(SensorTestModel.Level1))
                                             .WithFriendlyName("Humidifier")
                                             .WithNodeId("level_1"))
              .HasDehumidifierHumidifier(config => config.OnModel(model)
                                                         .WithStatusProperty(nameof(SensorTestModel.Level2))
                                                         .WithFriendlyName("Dehumidifier")
                                                         .WithNodeId("level_2"));
        return device;
    }

    private static DeviceBuilder ConfigureDevice4(SwitchTestModel model)
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Switch Test Device")
                                        .WithId("net2hassmqtt_test_device_04");

        device.HasSwitch(config => config.OnModel(model)
                                         .WithStatusProperty(nameof(SwitchTestModel.Switch1))
                                         .WithFriendlyName("Switch 1")
                                         .WithNodeId("switch_1"));

        device.HasOutletSwitch(config => config.OnModel(model)
                                               .WithStatusProperty(nameof(SwitchTestModel.Switch2))
                                               .WithFriendlyName("Outlet 1")
                                               .WithNodeId("switch_2")); // todo - why does this not show as an outlet in Home Assistant?

        return device;
    }

    private static DeviceBuilder ConfigureDevice3(SensorTestModel model)
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Number Test Device")
                                        .WithId("net2hassmqtt_test_device_03");

        device.HasNumber(config => config.OnModel(model)
                                         .WithStatusProperty(nameof(SensorTestModel.Level1))
                                         .WithCommandMethod(nameof(SensorTestModel.Level1CommandHandler))
                                         .WithFriendlyName("Level 1")
                                         .WithNodeId("level_1"))
              .HasCurrentNumber(config => config.OnModel(model)
                                                .WithStatusProperty(nameof(SensorTestModel.Current))
                                                .WithCommandMethod(nameof(SensorTestModel.CurrentCommandHandler))
                                                .WithFriendlyName("Status 1")
                                                .WithNodeId("status_1")
                                                .WithUnitOfMeasurement(CurrentNumberUoM.Ampere))
              .HasTemperatureNumber(config => config.OnModel(model)
                                                    .WithStatusProperty(nameof(SensorTestModel.Temperature))
                                                    .WithCommandMethod(nameof(SensorTestModel.TemperatureCommandHandler))
                                                    .WithFriendlyName("Temperature")
                                                    .WithNodeId("temperature")
                                                    .WithUnitOfMeasurement(TemperatureNumberUoM.DegreesCelsius));
        return device;
    }

    private static DeviceBuilder ConfigureDevice2(SensorTestModel model)
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Sensor Test Device")
                                        .WithId("net2hassmqtt_test_device_02");

        device.HasSensor(config => config.OnModel(model)
                                         .WithStatusProperty(nameof(SensorTestModel.Level1))
                                         .WithFriendlyName("Level 1")
                                         .WithNodeId("level_1"))
              .HasCurrentSensor(config => config.OnModel(model)
                                                .WithStatusProperty(nameof(SensorTestModel.Current))
                                                .WithFriendlyName("Status 1")
                                                .WithNodeId("status_1")
                                                .WithUnitOfMeasurement(CurrentSensorUoM.Ampere))
              .HasTemperatureSensor(config => config.OnModel(model)
                                                    .WithStatusProperty(nameof(SensorTestModel.Temperature))
                                                    .WithFriendlyName("Temperature")
                                                    .WithNodeId("temperature")
                                                    .WithUnitOfMeasurement(TemperatureSensorUoM.DegreesCelsius));
        return device;
    }

    private static DeviceBuilder ConfigureDevice1(BinarySensorTestModel model)
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Binary Sensor Test Device")
                                        .WithId("net2hassmqtt_test_device_01");

        device.HasDoorBinarySensor(config => config.OnModel(model)
                                                   .WithStatusProperty(nameof(BinarySensorTestModel.DoorStatus))
                                                   .WithFriendlyName("My friendly name")
                                                   .WithNodeId("entity_1234")
                                                   .WithUnitOfMeasurement(DoorBinarySensorUoM.None))
              .HasBinarySensor(config => config.OnModel(model)
                                               .WithStatusProperty(nameof(BinarySensorTestModel.Status1))
                                               .WithFriendlyName("Status 1")
                                               .WithNodeId("status_1"))
              .HasBinarySensor(config => config.OnModel(model)
                                               .WithStatusProperty(nameof(BinarySensorTestModel.Status2))
                                               .WithFriendlyName("Status 2")
                                               .WithNodeId("status_2"))
              .HasDoorBinarySensor(config => config.OnModel(model)
                                                   .WithStatusProperty(nameof(BinarySensorTestModel.DoorStatus))
                                                   .WithUnitOfMeasurement(DoorBinarySensorUoM.None)
                                                   .WithFriendlyName("Door Status")
                                                   .WithNodeId("door_status"))
              .HasBatteryBinarySensor(config => config.OnModel(model)
                                                      .WithStatusProperty(nameof(BinarySensorTestModel.BatteryStatus))
                                                      .WithUnitOfMeasurement(BatteryBinarySensorUoM.None)
                                                      .WithFriendlyName("Battery Status")
                                                      .WithNodeId("battery_status"))
              .HasCarbonMonoxideBinarySensor(config => config.OnModel(model)
                                                             .WithStatusProperty(nameof(BinarySensorTestModel.CarbonMonoxideStatus))
                                                             .WithUnitOfMeasurement(CarbonMonoxideBinarySensorUoM.None)
                                                             .WithFriendlyName("CarbonMonoxide Status")
                                                             .WithNodeId("carbon_monoxide_status"))
              .HasGarageDoorBinarySensor(config => config.OnModel(model)
                                                         .WithStatusProperty(nameof(BinarySensorTestModel.GarageDoorStatus))
                                                         .WithUnitOfMeasurement(GarageDoorBinarySensorUoM.None)
                                                         .WithFriendlyName("Garage Door Status")
                                                         .WithNodeId("garage_door_status"));
        return device;
    }
}