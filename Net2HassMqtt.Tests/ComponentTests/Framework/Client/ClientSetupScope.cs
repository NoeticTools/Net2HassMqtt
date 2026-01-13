using FluentDate;
using Moq;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;


namespace Net2HassMqtt.Tests.ComponentTests.Framework.Client;

public class ClientSetupScope(Mock<IManagedMqttClient> managedMqttClient)
{
    private int _connectedEventCalledCount = 0;
    private int _disconnectedEventCalledCount = 0;

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

    // ReSharper disable once MemberCanBePrivate.Global
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
                    switch (isConnected)
                    {
                        case true when !priorIsConnected:
                            managedMqttClient.Setup(x => x.IsConnected)
                                             .Callback(OnIsConnected)
                                             .Returns(isConnected);
                            break;

                        case false when priorIsConnected:
                            managedMqttClient.Setup(x => x.IsConnected)
                                             .Callback(OnIsDisconnected)
                                             .Returns(isConnected);
                            break;

                        default:
                            managedMqttClient.Setup(x => x.IsConnected)
                                             .Returns(isConnected);
                            break;
                    }

                    priorIsConnected = isConnected;
                }

                break;
            }
        }
    }

    private void OnIsConnected()
    {
        if (_connectedEventCalledCount++ != 1)
        {
            return;
        }

        _connectedEventCalledCount = 10;
        Task.Run(() => managedMqttClient.RaiseAsync(c => c.ConnectedAsync += null,
                                                    (new MqttClientConnectedEventArgs(new MqttClientConnectResult())))
                ).Wait(1.Seconds());
    }

    private void OnIsDisconnected()
    {
        if (_disconnectedEventCalledCount++ == 1)
        {
            _disconnectedEventCalledCount = 10;
            Task.Run(() => managedMqttClient.RaiseAsync(c => c.DisconnectedAsync += null,
                                                        (new MqttClientDisconnectedEventArgs(true, 
                                                                                             new MqttClientConnectResult(), 
                                                                                             MqttClientDisconnectReason.UnspecifiedError,
                                                                                             "Test disconnection", 
                                                                                             [], 
                                                                                             null)))
                    ).Wait(1.Seconds());
        }
    }
}