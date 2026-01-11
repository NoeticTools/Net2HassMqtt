using Moq;
using MQTTnet.Extensions.ManagedClient;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;

public class ClientScope(Mock<IManagedMqttClient> managedMqttClient)
{
    public ClientVerificationScope Verify => new ClientVerificationScope(managedMqttClient);
}