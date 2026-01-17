using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt.Configuration.Building;

public sealed class HassMqttClientFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public HassMqttClientFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    ///     Sample MQTT options for testing. Creates a MQTT client Dotnet2HassMqtt_Test (with number suffix, default is 333).
    /// </summary>
    /// <remarks>
    ///     If using a public MQTT client first check if a client with this name exists and set the id number so yours is
    ///     unique.
    ///     Also check if there already is a "homeassistant" client and change the Home Assistant client ID if required.
    /// </remarks>
    public static ManagedMqttClientOptions CreateQuickStartOptions(string clientId, IConfigurationRoot config)
    {
        var brokerAddress = config["MqttBroker:Address"];
        var brokerUsername = config["MqttBroker:Username"];
        var brokerPassword = config["MqttBroker:Password"];

        if (brokerAddress == null || brokerUsername == null || brokerPassword == null)
        {
            throw new
                Net2HassMqttConfigurationException("Developer secrets have not been set 'MqttBroker:Address', 'MqttBroker:Username', or 'MqttBroker:Password'. See https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows#enable-secret-storage.");
        }

        return new ManagedMqttClientOptionsBuilder()
               .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
               .WithClientOptions(new MqttClientOptionsBuilder()
                                  .WithProtocolVersion(MqttProtocolVersion.V500)
                                  .WithClientId(clientId)
                                  .WithTcpServer(brokerAddress)
                                  .WithCredentials(brokerUsername, brokerPassword)
                                  .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                                  .WithWillTopic($"{clientId}/{MqttConstants.WillSubTopic}")
                                  .WithWillRetain()
                                  .WithWillPayload("offline")
                                  .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                                  .Build())
               .Build();
    }

    internal INet2HassMqttClient Create(ManagedMqttClientOptions config, IManagedMqttClient? client = null)
    {
        client ??= new MqttFactory().CreateManagedMqttClient();
        return new Net2HassMqttClient(client, config, _loggerFactory.CreateLogger<Net2HassMqttClient>());
    }
}