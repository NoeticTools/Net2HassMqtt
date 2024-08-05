using System.ComponentModel;
using FluentDate;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Mqtt;

public interface INet2HassMqttClient : INotifyPropertyChanged
{
    string ClientMqttId { get; }

    IHassMqttDiscoveryClient Discovery { get; }

    bool IsConnected { get; }

    event HassMqttClientConnectionEventHandlers.OnConnectionChangedHandlerAsync OnConnectedAsync;

    event HassMqttClientConnectionEventHandlers.OnConnectionChangedHandlerAsync OnDisconnectedAsync;

    Task StartAsync();

    Task StopAsync(bool cleanDisconnect = true);

    Task<bool> WaitForConnection(FluentTimeSpan seconds);

    internal Task PublishAsync(MqttTopic topic, string payload);

    internal Task PublishStatusAsync<T>(TopicBuilder topicBuilder, T status);
    
    internal Task PublishCommandAsync<T>(TopicBuilder topicBuilder, T status);

    internal Task SubscribeToSetCommandsAsync(string deviceId, IMqttSubscriber subscriber);
}