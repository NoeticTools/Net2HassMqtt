using System.Dynamic;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Entities;

// WIP - Refactoring. Structure likely to change considerably.
internal abstract class EventEntityBase<T> : EntityBase<T>
    where T : EntityConfigBase
{
    protected EventEntityBase(T config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger)
        : base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
    }

    public override Task StartAsync()
    {
        Config.SubscribeEvent?.Invoke(OnEventActivated);

        return Task.CompletedTask;
    }

    public override Task StopAsync()
    {
        Config.UnsubscribeEvent?.Invoke(OnEventActivated);
        return Task.CompletedTask;
    }

    private async void OnEventActivated()
    {
        Console.WriteLine("OnEventActivated");
        var topic = new TopicBuilder().WithComponent(Config.MqttTopicComponent)
                                      .WithNodeId(DeviceNodeId)
                                      .WithObjectId(Config.EntityNodeId);
        dynamic payload = new ExpandoObject();
        payload.event_type = "press";
        await MqttClient.PublishStatusAsync(topic, payload);
    }
}