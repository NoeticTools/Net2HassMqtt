using System.Diagnostics;
using FluentDate;
using Microsoft.Extensions.Configuration;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Net2HassMqtt.Tests.ComponentTests.Framework.Client;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;
using Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching.EntityMessages;
using Net2HassMqtt.Tests.Sensors.SampleEntityModels;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ComponentTestsBase
{
    protected const string DeviceFriendlyName = "Test Device 1";
    protected const string DeviceId = "test_device_01";
    protected const string ClientId = "net2hassmqtt_test_start";
    private IConfigurationRoot _appConfig = null!;
    private Mock<IManagedMqttClient> _managedMqttClient = null!;
    private Mock<IMqttClient> _mqttClient = null!;
    private List<MqttApplicationMessage> _publishedMessages = null!;
    protected ComponentTestModel Model = null!;
    protected DeviceBuilder DeviceBuilder = null!;

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

        _appConfig = new ConfigurationBuilder().AddUserSecrets<ClientConnectionComponentTests>().Build();
        DeviceBuilder = CreateDeviceBuilder();

        _publishedMessages = [];
        _mqttClient.Setup(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>(), It.IsAny<CancellationToken>()))
                   .Callback<MqttApplicationMessage, CancellationToken>((message, _) => _publishedMessages.Add(message));

        Client = new ClientScope(_managedMqttClient);
        PublishedMqttMessages = new MqttMessagesScope(_publishedMessages);
        MqttMessageMatchers = new TestSensorsMessageMqttMatchers(ClientId, DeviceFriendlyName, DeviceId);
    }

    protected TestSensorsMessageMqttMatchers MqttMessageMatchers { get; set; } = null!;

    internal MqttMessagesScope PublishedMqttMessages { get; private set; } = null!;

    internal ClientScope Client { get; private set; } = null!;

    private static DeviceBuilder CreateDeviceBuilder()
    {
        return new DeviceBuilder().WithFriendlyName(DeviceFriendlyName)
                                  .WithId(DeviceId);
    }

    private async Task<bool> RunApplication(Action loopAction, int runLoopCount)
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

            result = await RunApplication(loopAction, runLoopCount);

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

    protected void BaseTearDown()
    {
        Task.Delay(10.Milliseconds()).Wait(); // todo: code smell, required to avoid intermittency in number of messages received
        // fix in separate issue
    }

    protected async Task RunMqttMessaging<T>(string entityFriendlyName, 
                                           string nodeId, 
                                           string deviceClassName, 
                                           string hassUnitOfMeasurement,
                                           Action loopAction, Func<int, T> getExpectedValue)
    {
        var result = await Run(loopAction, 2);

        Client.Verify
              .WasStartedOnce()
              .SubscriptionsCountIs(1);

        var mqttMsgMatcher = new SensorEntityMqttMessages(ClientId, DeviceId, DeviceFriendlyName,
                                                          nodeId,
                                                          entityFriendlyName,
                                                          deviceClassName,
                                                          hassUnitOfMeasurement);

        PublishedMqttMessages.Verify.MatchSequence(
        [
            MqttMessageMatchers.BridgeState.Online,
            mqttMsgMatcher.Config,
            mqttMsgMatcher.SendsState(getExpectedValue(1)),
            mqttMsgMatcher.SendsState(getExpectedValue(2)),
            MqttMessageMatchers.Any(9)
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }

    protected async Task RunMqttMessaging<T1,T2>(EntityMqttMessagesTestData<T1, T2> data, string hassDeviceClassName)
        where T1 : UnitOfMeasurement
    {
        var hassUnitOfMeasurement = data.UnitOfMeasurement.HassUnitOfMeasurement == "none" ? "" : data.UnitOfMeasurement.HassUnitOfMeasurement;
        await RunMqttMessaging<T2>(data.EntityFriendlyName, data.NodeId,
                                   hassDeviceClassName,
                                   hassUnitOfMeasurement,
                                   data.LoopAction, 
                                   data.GetExpectedValue);
    }
}