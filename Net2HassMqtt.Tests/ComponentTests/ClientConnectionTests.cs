using Moq;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Net2HassMqtt.Tests.ComponentTests.Framework;


namespace Net2HassMqtt.Tests.ComponentTests;

[TestFixture]
public class ClientConnectionTests : ComponentTestsBase
{
    [SetUp]
    public void Setup()
    {
        base.BaseSetup();
        SetupBatteryChargingBinarySensor();
    }

    [Test]
    public async Task BrokerFailsToConnectTest()
    {
        ManagedMqttClient.SetupIsConnected(false);

        var result = await Run();

        ManagedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once());
        ManagedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Never());
        Validate(PublishedMessages).Sequence([]);
        Assert.That(result, Is.False, "Expected run to fail.");
    }

    [Test]
    public async Task BrokerSlowToConnectTest()
    {
        ManagedMqttClient.SetupIsConnected(false, false, true);

        var result = await Run();

        ManagedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once());
        ManagedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Once());
        Validate(PublishedMessages).Sequence([MqttMessageMatcher.BridgeStateOfflineMessage]);
        Assert.That(result, Is.True, "Expected run to pass.");
    }

    [Test]
    public async Task SendsBridgeOfflineWhenStoppedTest()
    {
        ManagedMqttClient.SetupIsConnected();

        var result = await Run(ToggleChargingStatus, 5);

        ManagedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once);
        ManagedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Once());

        Validate(PublishedMessages).Sequence([
            MqttMessageMatcher.BatteryChargingStateOffMessage,
            MqttMessageMatcher.BatteryChargingStateOnMessage,
            MqttMessageMatcher.BatteryChargingStateOffMessage,
            MqttMessageMatcher.BatteryChargingStateOnMessage,
            MqttMessageMatcher.BatteryChargingStateOffMessage,
            MqttMessageMatcher.BridgeStateOfflineMessage,
        ]);

        Assert.That(result, Is.True, "Expected run to pass.");
    }
}