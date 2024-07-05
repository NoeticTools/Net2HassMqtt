# Net2HassMqtt

![Current Version](https://img.shields.io/badge/version-v0.1-blue)
![GitHub contributors](https://img.shields.io/github/contributors/madhur-taneja/README-Template)

> [!NOTE]  
> This project is currently experimental/pre-release with much of the code untested.
> Early trial and feedback would be great!

Net2HassMqtt provides [Home Assistant](https://www.home-assistant.io/) (HASS) integration to .NET applications via [MQTT](https://mqtt.org/).
It not a MQTT transport layer. It provides a Home Assistant centric fluent configuration interface to map your application's models to Home Assistant entities.

The devices and entities you configure automatically appear fully configured in Home Assistant ([Home Assistant MQTT Discovery](https://www.home-assistant.io/integrations/mqtt/#mqtt-discovery)).
Configure once, use twice & no YAML! :-)

You do not need to code any MQTT publish calls, value conversions, subscriptions, or connection management. 
It just all happens. Net2HassMqtt layers over [MQTT](https://mqtt.org/) so you do not have to. 
It is designed to let you work with the _"what"_ (Home Assistant) not the _"how"_ (MQTT).

An example:
```csharp
// Your application model(s)
var environment = new EnvironmentSensorModel();

// MQTT broker connection options
var mqttClientOptions = ExampleMqttClientOptions.GetOptions("NET2HassMqtt_NodeIdExample", secretsConfig);

// Now we map your app's model to HASS device and entities.
var config = new BridgeConfiguration(mqttClientOptions)

    .HasDevice("Demo Home Environment",
                "my_home_environment", 

                device => device.Model(environment)

                                // Map model 'Temperature' property to HASS entity - "Lounge Temperature"
                                .HasTemperatureSensor(nameof(EnvironmentSensorModel.Temperature),
                                                      TemperatureSensorUoM.DegreesCelsius,
                                                      "Lounge Temperature", "lounge_temperature")

                                // Map model 'Humidity' property to HASS entity - "Lounge Humidiy"
                                .HasHumiditySensor(nameof(EnvironmentSensorModel.Humidity),
                                                   HumiditySensorUoM.Percent,
                                                   "Lounge Humidity", "lounge_humidity"));

var bridge = config.Build();

await bridge.StartAsync();  // Done! Running.
```