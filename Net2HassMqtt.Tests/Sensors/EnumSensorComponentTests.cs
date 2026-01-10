using FluentDate;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using System.Diagnostics;
using MQTTnet;


namespace Net2HassMqtt.Tests.Sensors;

[TestFixture]
public class EnumSensorComponentTests
{
    private int _runLoopCount = 5;
    private Mock<IManagedMqttClient> _managedMqttClient;
    private ComponentTestModel _model;
    private DeviceBuilder _device;
    private IConfigurationRoot _appConfig;
    private Mock<IMqttClient> _mqttClient;

    [SetUp]
    public void Setup()
    {
        _runLoopCount = 5;
        _mqttClient = new Mock<IMqttClient>();

        _managedMqttClient = new Mock<IManagedMqttClient>(MockBehavior.Strict);
        _managedMqttClient.SetupGet(x => x.InternalClient).Returns(_mqttClient.Object);
        _managedMqttClient.Setup<Task>(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>())).Returns(Task.CompletedTask);
        _managedMqttClient.Setup<Task>(x => x.StopAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        _managedMqttClient.Setup(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()))
                          .Returns(Task.CompletedTask);

        _model = new ComponentTestModel
        {
            BatteryCharging = true,
            CurrentState = ComponentTestModel.TestStates.StateOne
        };

        _appConfig = new ConfigurationBuilder().AddUserSecrets<EnumSensorComponentTests>().Build();
        _device = CreateDevice();
    }

    [TearDown]
    public void TearDown()
    {
    }

    [Test]
    public async Task BrokerFailsToConnectTest()
    {
        _managedMqttClient.SetupGet(x => x.IsConnected)
                          .Returns(false);

        var result = await Run();

        _managedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once());
        _managedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Never());
        Assert.That(result, Is.False, "Expected run to fail.");
    }

    [Test]
    public async Task BrokerSlowToConnectTest()
    {
        _managedMqttClient.SetupSequence(x => x.IsConnected)
                          .Returns(false)
                          .Returns(false);
        _managedMqttClient.SetupGet(x => x.IsConnected)
                          .Returns(true)
                          .Raises(x => x.ConnectedAsync += null, new MqttClientConnectedEventArgs(new MqttClientConnectResult()));
        _managedMqttClient.SetupGet(x => x.IsConnected)
                          .Returns(true);

        var result = await Run();

        _managedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once());
        _managedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Once());
        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task BrokerConnectsImmediatelyTest()
    {
        _managedMqttClient.SetupGet(x => x.IsConnected).Returns(true);
        _runLoopCount = 5;                               

        var result = await Run();

        _managedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once);
        _managedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Once());
        _mqttClient.Verify(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(6));
        Assert.That(result, Is.True, "Expected run to pass.");
    }

    private async Task<bool> Run()
    {
        var mqttOptions = HassMqttClientFactory.CreateQuickStartOptions("net2hassmqtt_test_start", _appConfig);
        var bridge = new BridgeConfiguration()
                     .WithMqttOptions(mqttOptions)
                     .HasDevice(_device)
                     .Build(_managedMqttClient.Object);

        bool result;
        try
        {
            if (!await bridge.StartAsync())
            {
                return false;
            }

            result = await Run(_runLoopCount);

            await bridge.StopAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return result;
    }

    private DeviceBuilder CreateDevice()
    {
        var device = new DeviceBuilder().WithFriendlyName("Net2HassMqtt Component Test Device 1")
                                        .WithId("net2hassmqtt_component_test_device_01");

        device.HasBatteryChargingBinarySensor(config => config.OnModel(_model)
                                                              .WithStatusProperty(nameof(ComponentTestModel.BatteryCharging))
                                                              .WithFriendlyName("Battery Charging Status")
                                                              .WithNodeId("battery_1_charging"));

        //_device.HasEnumSensor(config => config.OnModel(_model)
        //                                     .WithStatusProperty(nameof(ComponentTestModel.TestStates))
        //                                     .WithFriendlyName("Current State")
        //                                     .WithNodeId("current_state"));
        return device;
    }

    private async Task<bool> Run(int runLoopCount)
    {
        var runTimeLimit = 3.Seconds();
        var stopwatch = Stopwatch.StartNew();

        while (runLoopCount-- > 0)
        {
            if (stopwatch.Elapsed > runTimeLimit)
            {
                return false;
            }

            await Task.Delay(500.Milliseconds());

            _model.BatteryCharging = !_model.BatteryCharging;

            //if (Console.KeyAvailable)
            //{
            //    var key = Console.ReadKey();
            //    if (key.KeyChar == 'x')
            //    {
            //        break;
            //    }

            //    if (key.KeyChar == '1')
            //    {
            //        model.BatteryCharging = !model.BatteryCharging;
            //    }

            //    model.OnKeyPressed(key.KeyChar);
            //}
        }

        return true;
    }

}
