using System.Diagnostics;
using FluentDate;
using Microsoft.Extensions.Configuration;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ComponentTestsBase
{
    protected Mock<IManagedMqttClient> ManagedMqttClient = null!;
    protected ComponentTestModel Model = null!;
    protected Mock<IMqttClient> MqttClient = null!;
    protected DeviceBuilder DeviceBuilder = null!;
    private IConfigurationRoot _appConfig = null!;
    protected List<MqttApplicationMessage> PublishedMessages = null!;

    protected static MqttMessagesValidationScope Validate(List<MqttApplicationMessage> messages)
    {
        return new MqttMessagesValidationScope(messages);
    }

    protected void BaseSetup()
    {
        MqttClient = new Mock<IMqttClient>();

        ManagedMqttClient = new Mock<IManagedMqttClient>(MockBehavior.Strict);
        ManagedMqttClient.SetupGet(x => x.InternalClient).Returns(MqttClient.Object);
        ManagedMqttClient.Setup<Task>(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>())).Returns(Task.CompletedTask);
        ManagedMqttClient.Setup<Task>(x => x.StopAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        ManagedMqttClient.Setup(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()))
                          .Returns(Task.CompletedTask);

        Model = new ComponentTestModel
        {
            BatteryCharging = true,
            CurrentState = ComponentTestModel.TestStates.StateOne
        };

        _appConfig = new ConfigurationBuilder().AddUserSecrets<ClientConnectionTests>().Build();
        DeviceBuilder = CreateDeviceBuilder();

        PublishedMessages = new List<MqttApplicationMessage>();
        MqttClient.Setup(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>(), It.IsAny<CancellationToken>()))
                  .Callback<MqttApplicationMessage, CancellationToken>((message, _) => PublishedMessages.Add(message));
    }

    private DeviceBuilder CreateDeviceBuilder()
    {
        return new DeviceBuilder().WithFriendlyName("Net2HassMqtt Component Test Device 1")
                                  .WithId("net2hassmqtt_component_test_device_01");
    }

    protected void SetupBatteryChargingBinarySensor()
    {
        DeviceBuilder.HasBatteryChargingBinarySensor(config => config.OnModel(Model)
                                                                     .WithStatusProperty(nameof(ComponentTestModel.BatteryCharging))
                                                                     .WithFriendlyName("Battery Charging Status")
                                                                     .WithNodeId("battery_1_charging"));
    }

    protected void SetupEnumSensor()
    {
        DeviceBuilder.HasEnumSensor(config => config.OnModel(Model)
                                             .WithStatusProperty(nameof(ComponentTestModel.TestStates))
                                             .WithFriendlyName("Current State")
                                             .WithNodeId("current_state"));
    }

    protected static async Task<bool> RunApplication(int runLoopCount, Action loopAction)
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

    protected async Task<bool> Run(Action loopAction, int runLoopCount)
    {
        var mqttOptions = HassMqttClientFactory.CreateQuickStartOptions("net2hassmqtt_test_start", _appConfig);
        var bridge = new BridgeConfiguration()
                     .WithMqttOptions(mqttOptions)
                     .HasDevice(DeviceBuilder)
                     .Build(ManagedMqttClient.Object);

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

    protected async Task<bool> Run()
    {
        return await Run(() => { }, 5);
    }

    protected void ToggleChargingStatus()
    {
        Model.BatteryCharging = !Model.BatteryCharging;
    }
}