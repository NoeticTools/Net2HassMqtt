using Moq;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ClientVerificationScope(Mock<IManagedMqttClient> managedMqttClient)
{
    public ClientVerificationScope WasStartedOnce()
    {
        managedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once);
        return this;
    }

    public ClientVerificationScope NoSubscriptionsMade()
    {
        return SubscriptionsCountIs(0);
    }

    public ClientVerificationScope SubscriptionsCountIs(int count)
    {
        managedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Exactly(count));
        return this;
    }
}