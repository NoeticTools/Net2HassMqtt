using FluentDate;
using Microsoft.Extensions.Logging;
using Moq;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Client;

public class ClientSetupScope(Mock<IManagedMqttClient> managedMqttClient)
{
    public void ConnectsAfterDelay()
    {
        IsConnected (false, false, false, true);
    }

    public void ConnectsImmediately()
    {
        IsConnected (true);
    }

    public void NeverConnects()
    {
        IsConnected (false);
    }

    public void IsConnected(params bool[] isConnectedSequence)
    {
        var priorIsConnected = false;

        switch (isConnectedSequence.Length)
        {
            case 0:
                managedMqttClient.Setup(x => x.IsConnected)
                                 .Callback(OnIsConnected)
                                 .Returns(true);
                break;

            case > 0:
            {
                foreach (var isConnected in isConnectedSequence)
                {
                    if (isConnected && !priorIsConnected)
                    {
                        managedMqttClient.Setup(x => x.IsConnected)
                                         .Callback(OnIsConnected)
                                         .Returns(true);
                    }
                    else
                    {
                        managedMqttClient.Setup(x => x.IsConnected)
                                         .Returns(isConnected);
                    }
                    priorIsConnected = isConnected;
                }

                break;
            }
        }
    }

    private int _isConnectedDelayCount = 0;

    private void OnIsConnected()
    {
        if (_isConnectedDelayCount++ == 0)
        {
            Console.WriteLine("--- on connected ---");
            Task.Run(() => managedMqttClient.RaiseAsync(c => c.ConnectedAsync += null,
                                                        (new MqttClientConnectedEventArgs(new MqttClientConnectResult())))
                    ).Wait(1.Seconds());
        }
        else
        {
            Console.WriteLine("--- connected ---");
        }
    }
}