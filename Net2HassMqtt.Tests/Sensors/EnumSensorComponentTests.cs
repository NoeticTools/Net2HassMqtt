using FluentDate;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using MQTTnet.Extensions.ManagedClient;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;


namespace Net2HassMqtt.Tests.Sensors;

[TestFixture]
public class EnumSensorComponentTests
{
    private IManagedMqttClient _managedMqttClient;
    private DeviceBuilder _device;
    private ComponentTestModel _model;

    [SetUp]
    public void Setup()
    {
        _managedMqttClient = Mock.Of<IManagedMqttClient>();

        _device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Component Test Device 1")
                                        .WithId("net2hassmqtt_component_test_device_01");

        _model = new ComponentTestModel
        {
            BatteryCharging = true,
            CurrentState = ComponentTestModel.TestStates.StateOne
        };
    }

    [Test]
    public async Task Test()
    {
        //device.HasBatteryChargingBinarySensor(config => config.OnModel(model)
        //                                                      .WithStatusProperty(nameof(ComponentTestModel.BatteryCharging))
        //                                                      .WithFriendlyName("Battery Charging Status")
        //                                                      .WithNodeId("battery_1_charging"));

        _device.HasEnumSensor(config => config.OnModel(_model)
                                             .WithStatusProperty(nameof(ComponentTestModel.TestStates))
                                             .WithFriendlyName("Current State")
                                             .WithNodeId("current_state"));

        await Run(_device, _model);
    }

    private async Task Run(DeviceBuilder device, ComponentTestModel model)
    {
        var appConfig = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

        var mqttOptions = HassMqttClientFactory.CreateQuickStartOptions("net2hassmqtt_test_start", appConfig);
        var bridge = new BridgeConfiguration()
                     .WithMqttOptions(mqttOptions)
                     .HasDevice(device)
                     .Build(_managedMqttClient);

        await bridge.StartAsync();
        await Run(model);
        await bridge.StopAsync();
    }

    private static async Task Run(ComponentTestModel model)
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

                model.OnKeyPressed(key.KeyChar);
            }
        }
    }}
