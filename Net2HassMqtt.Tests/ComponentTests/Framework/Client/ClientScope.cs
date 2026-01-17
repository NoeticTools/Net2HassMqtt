using Moq;
using MQTTnet.Extensions.ManagedClient;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Client;

public class ClientScope(Mock<IManagedMqttClient> managedMqttClient)
{
    public ClientSetupScope Setup => new(managedMqttClient);

    public ClientVerificationScope Verify => new(managedMqttClient);
}