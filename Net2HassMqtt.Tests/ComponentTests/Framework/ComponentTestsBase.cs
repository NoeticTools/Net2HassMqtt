using FluentDate;
using Microsoft.Extensions.Configuration;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Net2HassMqtt.Tests.ComponentTests.Framework.Client;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using System.Diagnostics;
using Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ComponentTestsBase
{
    private const string DeviceFriendlyName = "Test Device 1";
    private const string DeviceId = "test_device_01";
    private Mock<IManagedMqttClient> _managedMqttClient = null!;
    protected ComponentTestModel Model = null!;
    private Mock<IMqttClient> _mqttClient = null!;
    protected DeviceBuilder DeviceBuilder = null!;
    private IConfigurationRoot _appConfig = null!;
    private List<MqttApplicationMessage> _publishedMessages = null!;

    protected void BaseSetup()
    {
        _mqttClient = new Mock<IMqttClient>();

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

    protected DeviceMessageMatchers MessageMatchers { get; } = new("net2hassmqtt_test_start", DeviceFriendlyName, DeviceId);

    internal MqttMessagesScope PublishedMessages  { get; private set; } = null!;

    internal ClientScope Client { get; private set; } = null!;

    private static DeviceBuilder CreateDeviceBuilder()
    {
        return new DeviceBuilder().WithFriendlyName(DeviceFriendlyName)
                                  .WithId(DeviceId);
    }

    private async Task<bool> RunApplication(int runLoopCount, Action loopAction)
    {
        var runTimeLimit = 2.Seconds();
        var stopwatch = Stopwatch.StartNew();

        while (runLoopCount-- > 0)
        {
            if (stopwatch.Elapsed > runTimeLimit || !_managedMqttClient.Object.IsConnected)
            {
                return false;
            }

            loopAction.Invoke();
            await Task.Delay(5.Milliseconds());
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
            await Task.Delay(5.Milliseconds()); // todo - timing hack

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
}