using Microsoft.Extensions.Configuration;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Examples1.SampleEntityModels;


namespace NoeticTools.Net2HassMqtt.Examples1;

internal class NodeIdsConfigurationExample
{
    /// <summary>
    ///     An example code fragment demonstrating node IDs when configuring entity maps.
    /// </summary>
    /// <param name="appConfig"></param>
    /// <remarks>
    ///     <para>The following will create a configuration with:</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <para>
    ///                 An MQTT client "net2hassmqtt_examples" with a device
    ///                 named "Demo Lounge Environment" that has two entities:
    ///             </para>
    ///             <list type="bullet">
    ///                 <item>"Lounge Temperature"</item>
    ///                 <item>"Lounge Humidity"</item>
    ///             </list>
    ///         </item>
    ///     </list>
    ///     The entities will have entity IDs:
    ///     <list type="bullet">
    ///         <item>
    ///             <code>sensor.my_home_environment_lounge_temperature</code>
    ///         </item>
    ///         <item>
    ///             <code>sensor.my_home_environment_lounge_humidity</code>
    ///         </item>
    ///     </list>
    /// </remarks>
    public static INet2HassMqttBridge Example(IConfigurationRoot appConfig)
    {
        var environment = new EnvironmentSensorModel(); // the application's entity model
        var bridge = BuildBridge(appConfig, environment);
        return bridge;
    }

    private static INet2HassMqttBridge BuildBridge(IConfigurationRoot appConfig, EnvironmentSensorModel environment)
    {
        var mqttClientOptions = HassMqttClientFactory.CreateQuickStartOptions("net2hassmqtt_examples", appConfig);

        // Create a device to hold the entities
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Example Device 1")
                                        .WithId("net2hassmqtt_example_device_01");

        // Map entities
        device.HasTemperatureSensor(config => config.OnModel(environment)
                                                    .WithStatusProperty(nameof(EnvironmentSensorModel.Temperature))
                                                    .WithUnitOfMeasurement(TemperatureSensorUoM.DegreesCelsius)
                                                    .WithFriendlyName("Lounge Temperature")
                                                    .WithNodeId("lounge_temperature"))
              .HasBatteryBinarySensor(config => config.OnModel(environment)
                                                      .WithStatusProperty(nameof(EnvironmentSensorModel.TestBool))
                                                      .WithFriendlyName("Test")
                                                      .WithNodeId("test")
                                                      .WithUnitOfMeasurement(BatteryBinarySensorUoM.None))
              .HasHumiditySensor(config => config.OnModel(environment)
                                                 .WithStatusProperty(nameof(EnvironmentSensorModel.Humidity))
                                                 .WithUnitOfMeasurement(HumiditySensorUoM.Percent)
                                                 .WithFriendlyName("Lounge Humidity")
                                                 .WithNodeId("lounge_humidity"));

        // Build bridge
        var bridge = new BridgeConfiguration().WithMqttOptions(mqttClientOptions)
                                              .HasDevice(device)
                                              .Build();

        return bridge;
    }
}