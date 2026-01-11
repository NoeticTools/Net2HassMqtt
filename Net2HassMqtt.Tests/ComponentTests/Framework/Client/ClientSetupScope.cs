using Moq;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Client;

public class ClientSetupScope(Mock<IManagedMqttClient> managedMqttClient)
{
    public void IsConnected(params bool[] isConnectedSequence)
    {
        var priorIsConnected = false;

        switch (isConnectedSequence.Length)
        {
            case 0:
                SetupOnConnectedBehaviour();
                break;
            case > 0:
            {
                foreach (var isConnected in isConnectedSequence)
                {
                    if (isConnected && !priorIsConnected)
                    {
                        SetupOnConnectedBehaviour();
                    }
                    else
                    {
                        managedMqttClient.SetupGet(x => x.IsConnected).Returns(isConnected);
                    }
                    priorIsConnected = isConnected;
                }

                break;
            }
        }
    }

    private void SetupOnConnectedBehaviour()
    {
        managedMqttClient.SetupGet(x => x.IsConnected)
                         .Returns(true)
                         .Raises(x => x.ConnectedAsync += null,
                                 new MqttClientConnectedEventArgs(new MqttClientConnectResult()));
        managedMqttClient.SetupGet(x => x.IsConnected)
                         .Returns(true);
    }
}