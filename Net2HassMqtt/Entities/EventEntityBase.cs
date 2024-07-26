using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;
using static System.Net.Mime.MediaTypeNames;


namespace NoeticTools.Net2HassMqtt.Entities;

internal abstract class EventEntityBase<T> : EntityBase<T>
    where T : EntityConfigBase
{
    private readonly EventInfo _eventInfo;
    private readonly Delegate _eventHandlerDelegate;

    protected EventEntityBase(T config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger)
        : base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
        _eventInfo = GetModelEventInfo();
        var type = _eventInfo.EventHandlerType!;
        _eventHandlerDelegate = Delegate.CreateDelegate(type, this, nameof(OnEvent));
    }

    private EventInfo GetModelEventInfo()
    {
        if (Config.Model == null)
        {
            ThrowConfigError($"An event requires a model.");
        }

        if (string.IsNullOrWhiteSpace(Config.EventMemberName))
        {
            ThrowConfigError($"An event requires a model's event member name.");
        }

        var model = Config.Model;
        var eventInfo = model.GetType().GetEvent(Config.EventMemberName, BindingFlags.Instance | BindingFlags.Public);
        if (eventInfo != null)
        {
            return eventInfo!;
        }

        {
            var message = $"Could not find public method '{Config.EventMemberName}' on model of type of type '{model.GetType()}'";
            Logger.LogError(message);
            throw new Net2HassMqttConfigurationException(message);
        }
    }

    [DoesNotReturn]
    private void ThrowConfigError(string message)
    {
        Logger.LogError(message);
        throw new Net2HassMqttConfigurationException(message);
    }

    public override Task StartAsync()
    {
        _eventInfo.AddEventHandler(Config.Model, _eventHandlerDelegate);
        return Task.CompletedTask;
    }

    public override Task StopAsync()
    {
        _eventInfo.RemoveEventHandler(Config.Model, _eventHandlerDelegate);
        return Task.CompletedTask;
    }

    internal void OnEvent(object sender, EventArgs args)
    {
        Console.WriteLine("OnEventActivated");
        var topic = new TopicBuilder().WithComponent(Config.MqttTopicComponent)
                                      .WithNodeId(DeviceNodeId)
                                      .WithObjectId(Config.EntityNodeId);
        dynamic payload = new ExpandoObject();
        payload.event_type = "press";
        var _ = MqttClient.PublishStatusAsync(topic, payload);
    }
}