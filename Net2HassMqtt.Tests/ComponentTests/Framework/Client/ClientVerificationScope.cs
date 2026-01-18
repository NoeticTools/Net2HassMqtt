using Moq;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Client;

public class ClientVerificationScope(Mock<IManagedMqttClient> managedMqttClient)
{
    public ClientVerificationScope NoSubscriptionsMade()
    {
        return SubscriptionsCountIs(0);
    }

    public ClientVerificationScope SubscriptionsCountIs(int count)
    {
        managedMqttClient.Verify(x => x.SubscribeAsync(It.IsAny<IEnumerable<MqttTopicFilter>>()), Times.Exactly(count));
        return this;
    }

    public ClientVerificationScope WasStartedOnce()
    {
        managedMqttClient.Verify(x => x.StartAsync(It.IsAny<ManagedMqttClientOptions>()), Times.Once);
        return this;
    }
}