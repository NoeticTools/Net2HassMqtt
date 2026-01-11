using System.Diagnostics;
using System.Text;
using FluentDate;
using Microsoft.Extensions.Configuration;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using Net2HassMqtt.Tests.ComponentTests.Framework;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class EnumSensorComponentTests
{
    private Mock<IManagedMqttClient> _managedMqttClient;
    private ComponentTestModel _model;
    private DeviceBuilder _device;
    private IConfigurationRoot _appConfig;
    private Mock<IMqttClient> _mqttClient;
    
    private readonly MqttMessageMatcher _batteryChargingStateOffMessage =
        new("net2hassmqtt_test_start/net2hassmqtt_component_test_device_01/battery_1_charging",
            """
            {
              "attributes": {},
              "state": "OFF"
            }
            """);
    private readonly MqttMessageMatcher _batteryChargingStateOnMessage =
        new("net2hassmqtt_test_start/net2hassmqtt_component_test_device_01/battery_1_charging",
            """
            {
              "attributes": {},
              "state": "ON"
            }
            """);
    private readonly MqttMessageMatcher _bridgeStateOfflineMessage =
        new("net2hassmqtt_test_start/bridge/state",
            """
            {
              "state": "offline"
            }
            """);

    [SetUp]
    public void Setup()
    {
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
                          .Raises(x => x.ConnectedAsync += null, 
                                  new MqttClientConnectedEventArgs(new MqttClientConnectResult()));
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
        var publishedMessages = new List<MqttApplicationMessage>();
        _mqttClient.Setup(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>(), It.IsAny<CancellationToken>()))
                   .Callback<MqttApplicationMessage, CancellationToken>((message, _) => publishedMessages.Add(message));

        var result = await Run(ToggleChargingStatus, 5);

        _managedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once);
        _managedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Once());

        Validate(publishedMessages).Sequence([
            _batteryChargingStateOffMessage,
            _batteryChargingStateOnMessage,
            _batteryChargingStateOffMessage,
            _batteryChargingStateOnMessage,
            _batteryChargingStateOffMessage,
            _bridgeStateOfflineMessage,
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    private void ToggleChargingStatus()
    {
        _model.BatteryCharging = !_model.BatteryCharging;
    }

    private MqttMessagesValidationScope Validate(List<MqttApplicationMessage> messages)
    {
        var publishedMessage = messages[0];
        var payload = Encoding.UTF8.GetString(publishedMessage.PayloadSegment); 
        Assert.That(publishedMessage.Topic, Is.EqualTo("net2hassmqtt_test_start/net2hassmqtt_component_test_device_01/battery_1_charging"));
        Assert.That(payload, Is.EqualTo("""
                                        {
                                          "attributes": {},
                                          "state": "OFF"
                                        }
                                        """));
        Assert.That(publishedMessage.Retain, Is.True);
        Assert.That(publishedMessage.Dup, Is.False);
        Assert.That(publishedMessage.MessageExpiryInterval, Is.EqualTo(0));
        Assert.That(publishedMessage.PayloadFormatIndicator, Is.EqualTo(MqttPayloadFormatIndicator.Unspecified));
        Assert.That(publishedMessage.QualityOfServiceLevel, Is.EqualTo(MqttQualityOfServiceLevel.AtLeastOnce));
        Assert.That(publishedMessage.ContentType, Is.Null);
        Assert.That(publishedMessage.ResponseTopic, Is.Null);

        return new MqttMessagesValidationScope(messages);
    }

    private async Task<bool> Run()
    {
        return await Run(() => { }, 5);
    }

    private async Task<bool> Run(Action loopAction, int runLoopCount)
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

            result = await RunApplication(runLoopCount, loopAction);

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

    private static async Task<bool> RunApplication(int runLoopCount, Action loopAction)
    {
        var runTimeLimit = 3.Seconds();
        var stopwatch = Stopwatch.StartNew();

        while (runLoopCount-- > 0)
        {
            if (stopwatch.Elapsed > runTimeLimit)
            {
                return false;
            }

            await Task.Delay(5.Milliseconds());

            loopAction.Invoke();
        }

        return true;
    }

}