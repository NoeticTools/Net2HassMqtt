using FluentDate;
using Microsoft.Extensions.Configuration;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Net2HassMqtt.Tests.ComponentTests.Framework.ApplicationMessages;
using Net2HassMqtt.Tests.ComponentTests.Framework.Client;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.Mqtt;
using System.Diagnostics;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ComponentTestsBase
{
    private Mock<IManagedMqttClient> _managedMqttClient = null!;
    protected ComponentTestModel Model = null!;
    private Mock<IMqttClient> _mqttClient = null!;
    protected DeviceBuilder DeviceBuilder = null!;
    private IConfigurationRoot _appConfig = null!;
    private List<MqttApplicationMessage> _publishedMessages = null!;
    //private Mock<IHassMqttDiscoveryClient> _discoveryClient;

    protected void BaseSetup()
    {
        //_discoveryClient = new Mock<IHassMqttDiscoveryClient>();

        _mqttClient = new Mock<IMqttClient>();
        //_mqttClient.SetupGet(x => x.d)
        //xxx; // >>> mock Discovery IHassMqttDiscoveryClient

        _managedMqttClient = new Mock<IManagedMqttClient>(MockBehavior.Strict);
        _managedMqttClient.SetupGet(x => x.InternalClient).Returns(_mqttClient.Object);
        _managedMqttClient.Setup<Task>(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>())).Returns(Task.CompletedTask);
        _managedMqttClient.Setup<Task>(x => x.StopAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        _managedMqttClient.Setup(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()))
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

        Client = new ClientScope(_managedMqttClient);
        PublishedMessages = new MqttMessagesScope(_publishedMessages);
    }

    internal MqttMessagesScope PublishedMessages  { get; private set; } = null!;

    internal ClientScope Client { get; private set; } = null!;

    private static DeviceBuilder CreateDeviceBuilder()
    {
        return new DeviceBuilder().WithFriendlyName("Net2HassMqtt Component Test Device 1")
                                  .WithId("net2hassmqtt_component_test_device_01");
    }

    private async Task<bool> RunApplication(int runLoopCount, Action loopAction)
    {
        var runTimeLimit = 2.Seconds();
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

    protected async Task<bool> Run()
    {
        return await Run(() => { }, 5);
    }

    protected void ToggleChargingStatus()
    {
        Model.BatteryCharging = !Model.BatteryCharging;
    }
}