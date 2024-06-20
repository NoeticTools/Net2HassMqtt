using Examples.SampleEntityModels;
using Microsoft.Extensions.Configuration;
using NoeticTools.NET2HassMqtt.Configuration;
using NoeticTools.NET2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Examples
{
    internal class NodeIdsConfigurationExample
    {
        /// <summary>
        /// An example code fragment demonstrating node IDs when configuring entity maps.
        /// </summary>
        /// <remarks>
        /// <para>The following will create a configuration with:</para>
        ///
        /// <list type="bullet">
        ///     <item>
        ///         <para>An MQTT client "NET2HomeAssistantMqtt_NodeIdExample" with a device
        ///         named "Demo Lounge Environment" that has two entities:</para>
        ///
        ///         <list type="bullet">
        ///             <item>"Lounge Temperature"</item>
        ///             <item>"Lounge Humidity"</item>
        ///         </list>
        ///     </item>
        /// </list>
        /// 
        /// The entities will have entity IDs:
        ///
        /// <list type="bullet">
        ///    <item><code>sensor.my_home_environment_lounge_temperature</code></item>
        ///    <item><code>sensor.my_home_environment_lounge_humidity</code></item>
        /// </list>
        /// 
        /// </remarks>
        public void Example()
        {
            var secretsConfig = new ConfigurationBuilder()
                         .AddUserSecrets<NodeIdsConfigurationExample>()
                         .Build();

            var environment = new EnvironmentSensorModel(); // the application entity model
            var mqttClientOptions = ExampleMqttClientOptions.GetOptions("NET2HassMqtt_NodeIdExample", secretsConfig);

            var config = new BridgeConfiguration(mqttClientOptions)
                .HasDevice("Demo Home Environment",
                           "my_home_environment", device => device.Model(environment, "lounge")
                                                                  .HasTemperatureSensor(nameof(QuickStartDemoApp.SampleEntityModels.EnvironmentSensorModel.Temperature),
                                                                                        TemperatureSensorUoM.DegreesCelsius,
                                                                                        "Lounge Temperature", "temperature")

                                                                  .HasHumiditySensor(nameof(QuickStartDemoApp.SampleEntityModels.EnvironmentSensorModel.Humidity),
                                                                                     HumiditySensorUoM.Percent,
                                                                                     "Lounge Humidity", "humidity"));

            var bridge = config.Build();
            bridge.StartAsync();
        }
    }
}
