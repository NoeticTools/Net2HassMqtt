﻿using FluentDate;
using Microsoft.Extensions.Configuration;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp;

internal class Program
{
    private static bool _toggle;

    private static async Task Main(string[] args)
    {
        Console.WriteLine("""
                          Net2HassMqtt Quick Start Demo"

                          Press:
                            'x' to exit
                            '1' to toggle the state property
                            '2' to fire event A/B
                          
                          """);

        var appConfig = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        var model = new QuickStartDemoModel
        {
            BatteryCharging = true
        };

        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Quick Start Device 1")
                                        .WithId("net2hassmqtt_quick_start_device_01");

        device.HasBatteryChargingBinarySensor(config => config.OnModel(model)
                                                              .WithStatusProperty(nameof(QuickStartDemoModel.BatteryCharging))
                                                              .WithFriendlyName("Battery Charging Status")
                                                              .WithNodeId("battery_1_charging"));

        device.HasEvent(config => config.OnModel(model)
                                        .WithEvent(nameof(QuickStartDemoModel.TestEvent))
                                        .WithEventTypes(["A", "B"])
                                        .WithFriendlyName("Test Event")
                                        .WithNodeId("test_event")
                                        .WithAttribute(nameof(QuickStartDemoModel.ModelName), "Property attribute - read automatically"));


        var mqttOptions = HassMqttClientFactory.CreateQuickStartOptions("net2hassmqtt_quick_start", appConfig);
        var bridge = new BridgeConfiguration()
                     .WithMqttOptions(mqttOptions)
                     .HasDevice(device)
                     .Build();

        await bridge.StartAsync();
        await Run(model);
        await bridge.StopAsync();

        Console.WriteLine("Finished");
    }

    private static async Task Run(QuickStartDemoModel model)
    {
        while (true)
        {
            await Task.Delay(100.Milliseconds());
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'x')
                {
                    break;
                }

                if (key.KeyChar == '1')
                {
                    model.BatteryCharging = !model.BatteryCharging;
                }

                if (key.KeyChar == '2')
                {
                    _toggle = !_toggle;
                    model.FireEvent(_toggle ? "A" : "B");
                }
            }
        }
    }
}