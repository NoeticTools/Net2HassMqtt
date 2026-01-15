using FluentDate;
using Microsoft.Extensions.Configuration;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels;
using System.Reflection;
using static NoeticTools.Net2HassMqtt.QuickStartDemoApp.SampleEntityModels.QuickStartDemoModel;


namespace NoeticTools.Net2HassMqtt.QuickStartDemoApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("""
                          Net2HassMqtt Quick Start Demo"

                          Press:
                            'x' to exit
                            '1' to toggle the BatteryCharging state property
                            '2' to bump the CurrentState enum property
                            All key presses are sent to the demo model. 'a' and 'b' will fire events.
                          
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

        device.HasEnumSensor(config => config.OnModel(model)
                                                    .WithStatusProperty(nameof(QuickStartDemoModel.CurrentState))
                                                    .WithFriendlyName("Current State")
                                                    .WithNodeId("current_state"));

        device.HasEvent(config => config.OnModel(model)
                                        .WithEvent(nameof(QuickStartDemoModel.EfEvent))
                                        .WithEventTypes<Event3Types>()
                                        .WithFriendlyName("EF event (enum)")
                                        .WithNodeId("test_event_ef"));

        device.HasEvent(config => config.OnModel(model)
                                        .WithEvent(nameof(QuickStartDemoModel.AbEvent))
                                        .WithEventTypes(["PressedA", "PressedB"])
                                        .WithFriendlyName("AB event")
                                        .WithNodeId("test_event_ab")
                                        .WithAttribute(nameof(QuickStartDemoModel.ModelName), "Property attribute - read automatically"));

        device.HasEvent(config => config.OnModel(model)
                                        .WithEvent(nameof(QuickStartDemoModel.CdEvent))
                                        .WithEventTypes(["PressedC", "PressedD"])
                                        .WithEventTypeToSendAfterEachEvent("Clear")
                                        .WithFriendlyName("CD event (pulsed)")
                                        .WithNodeId("test_event_cd"));

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
                    BumpCurrentState(model);
                }

                model.OnKeyPressed(key.KeyChar);
            }
        }
    }

    private static void BumpCurrentState(QuickStartDemoModel model)
    {
        model.CurrentState = model.CurrentState switch
        {
            QuickStartDemoModel.CurrentStates.StateOne => QuickStartDemoModel.CurrentStates.StateTwo,
            QuickStartDemoModel.CurrentStates.StateTwo => QuickStartDemoModel.CurrentStates.StateThree,
            QuickStartDemoModel.CurrentStates.StateThree => QuickStartDemoModel.CurrentStates.StateOne,
            _ => model.CurrentState
        };
    }
}