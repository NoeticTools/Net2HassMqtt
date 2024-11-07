# Net2HassMqtt

Net2HassMqtt provides [Home Assistant](https://www.home-assistant.io/) (HASS) integration to .NET applications via [MQTT](https://mqtt.org/).
It not a MQTT transport layer. It provides a Home Assistant centric fluent configuration interface to map your application's models to Home Assistant entities.

The devices and entities you configure automatically appear fully configured in Home Assistant ([Home Assistant MQTT Discovery](https://www.home-assistant.io/integrations/mqtt/#mqtt-discovery)).
Configure once, use twice & no YAML! :-)

You do not need to code any MQTT publish calls, value conversions, subscriptions, or connection management. 
It just all happens. Net2HassMqtt layers over [MQTT](https://mqtt.org/) so you do not have to. 
It is designed to let you work with the _"what"_ (Home Assistant) not the _"how"_ (MQTT).

An example:

```csharp
    // Create a device to hold the entities
    var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Quick Start Device 1")
                                    .WithId("net2hassmqtt_quick_start_device_01");

    // Map model property to an entity
    device.HasBatteryChargingBinarySensor(config => config.OnModel(model)
                                                          .WithStatusProperty(nameof(QuickStartDemoModel.BatteryChaging))
                                                          .WithUnitOfMeasurement(BatteryChargingBinarySensorUoM.None)
                                                          .WithFriendlyName("Battery Charging Status")
                                                          .WithNodeId("battery_1_charging"));

    // Build bridge
    var bridge = new BridgeConfiguration().WithMqttOptions(mqttClientOptions)
                                          .HasDevice(device)
                                          .Build();

    // Run
    await bridge.StartAsync();
```