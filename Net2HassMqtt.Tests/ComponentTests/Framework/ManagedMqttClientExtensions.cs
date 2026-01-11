using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MQTTnet.Extensions.ManagedClient;


namespace Net2HassMqtt.Tests.ComponentTests.Framework;
internal static class ManagedMqttClientExtensions
{
    public static void SetupIsConnected(this Mock<IManagedMqttClient> client, params bool[] isConnectedSequence)
    {
        var priorIsConnected = false;

        if (isConnectedSequence.Length == 0)
        {
            client.SetupOnConnectedBehaviour();
        }
        else if (isConnectedSequence.Length > 0)
        {
            foreach (var isConnected in isConnectedSequence)
            {
                if (isConnected && !priorIsConnected)
                {
                    client.SetupOnConnectedBehaviour();
                }
                else
                {
                    client.SetupGet(x => x.IsConnected).Returns(isConnected);
                }
                priorIsConnected = isConnected;
            }
            //_managedMqttClient.SetupSequence(x => x.IsConnected)
            //                  .Returns(false)
            //                  .Returns(false);
            //_managedMqttClient.SetupGet(x => x.IsConnected)
            //                  .Returns(true)
            //                  .Raises(x => x.ConnectedAsync += null,
            //                          new MqttClientConnectedEventArgs(new MqttClientConnectResult()));
            //_managedMqttClient.SetupGet(x => x.IsConnected)
            //                  .Returns(true);
        }
    }

    private static void SetupOnConnectedBehaviour(this Mock<IManagedMqttClient> client)
    {
        client.SetupGet(x => x.IsConnected)
                          .Returns(true)
                          .Raises(x => x.ConnectedAsync += null,
                                  new MqttClientConnectedEventArgs(new MqttClientConnectResult()));
        client.SetupGet(x => x.IsConnected)
                          .Returns(true);
    }}
