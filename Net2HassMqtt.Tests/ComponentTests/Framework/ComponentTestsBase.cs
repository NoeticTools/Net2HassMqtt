using System.Diagnostics;
using FluentDate;
using Microsoft.Extensions.Configuration;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Net2HassMqtt.Tests.ComponentTests.Framework.ApplicationMessages;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ComponentTestsBase
{
    protected Mock<IManagedMqttClient> ManagedMqttClient = null!;
    protected ComponentTestModel Model = null!;
    private Mock<IMqttClient> _mqttClient = null!;
    protected DeviceBuilder DeviceBuilder = null!;
    private IConfigurationRoot _appConfig = null!;
    private List<MqttApplicationMessage> _publishedMessages = null!;

    protected void BaseSetup()
    {
        _mqttClient = new Mock<IMqttClient>();

        ManagedMqttClient = new Mock<IManagedMqttClient>(MockBehavior.Strict);
        ManagedMqttClient.SetupGet(x => x.InternalClient).Returns(_mqttClient.Object);
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

        _publishedMessages = [];
        _mqttClient.Setup(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>(), It.IsAny<CancellationToken>()))
                  .Callback<MqttApplicationMessage, CancellationToken>((message, _) => _publishedMessages.Add(message));

        Client = new ClientScope(ManagedMqttClient);
        PublishedMessages = new MqttMessagesScope(_publishedMessages);
    }

    internal MqttMessagesScope PublishedMessages  { get; private set; } = null!;

    internal ClientScope Client { get; private set; } = null!;

    private static DeviceBuilder CreateDeviceBuilder()
    {
        return new DeviceBuilder().WithFriendlyName("Net2HassMqtt Component Test Device 1")
                                  .WithId("net2hassmqtt_component_test_device_01");
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