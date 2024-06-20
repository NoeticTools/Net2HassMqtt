using Microsoft.Extensions.Configuration;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using NoeticTools.NET2HassMqtt.Mqtt;


namespace Examples;

internal class ExampleMqttClientOptions
{
    public static ManagedMqttClientOptions GetOptions(string clientId, IConfigurationRoot config)
    {
        return new ManagedMqttClientOptionsBuilder()
               .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
               .WithClientOptions(new MqttClientOptionsBuilder()
                                  .WithProtocolVersion(MqttProtocolVersion.V500)
                                  .WithClientId(clientId)
                                  .WithTcpServer(config["MqttBroker:Address"])
                                  .WithCredentials(config["MqttBroker:Username"], config["MqttBroker:Password"])
                                  .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                                  .WithWillTopic($"{MqttConstants.DefaultMqttClientId}/{MqttConstants.WillSubTopic}")
                                  .WithWillRetain()
                                  .WithWillPayload("offline")
                                  .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                                  .Build())
               .Build();
    }
}