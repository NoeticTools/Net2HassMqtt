using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

internal abstract class EventEntityBase<T> : EntityBase<T>
    where T : EventConfig
{
    private readonly Delegate _eventHandlerDelegate;
    private readonly EventInfo _eventInfo;

    protected EventEntityBase(T config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger)
        : base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
        _eventInfo = GetModelEventInfo();
        var type = _eventInfo.EventHandlerType!;
        _eventHandlerDelegate = Delegate.CreateDelegate(type, this, nameof(OnEventWithEventTypeName));

        //ThrowConfigError($"Unsupported event type '{_eventInfo.EventHandlerType.Name}'. Event type must be 'EventHandler<T>' where T : HassEventArgs.");
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

    internal void OnEventWithEventTypeName(object sender, HassEventArgs eventArgs)
    {
        if (string.IsNullOrWhiteSpace(eventArgs.EventType))
        {
            Logger.LogError("Event is missing required event type. Event not published.");
            return;
        }

        if (!Config.EventTypes.Contains(eventArgs.EventType))
        {
            Logger.LogError("Event type '{EventType}' is invalid. Valid event types are: {EventTypes}.",
                            eventArgs.EventType,
                            string.Join(", ", Config.EventTypes));
            return;
        }

        var attributes = GetAttributeValuesDictionary();
        foreach (var attribute in eventArgs.Attributes)
        {
            attributes.Add(attribute.Key, attribute.Value);
        }

        var payload = new EventWithAttributeDataMqttJson(eventArgs.EventType, attributes);
        var _ = PublishStatusAsync(payload);
    }

    private EventInfo GetModelEventInfo()
    {
        if (Config.Model == null)
        {
            ThrowConfigError("An event requires a model.");
        }

        if (string.IsNullOrWhiteSpace(Config.EventMemberName))
        {
            ThrowConfigError("An event requires a model's HaEvent member name.");
        }

        var model = Config.Model;
        
        var eventInfo = model.GetType().GetEvent(Config.EventMemberName, BindingFlags.Instance | BindingFlags.Public);
        if (eventInfo == null)
        {
            var message = $"Could not find public event '{Config.EventMemberName}.Event' on model of type '{model.GetType()}'";
            Logger.LogError(message);
            throw new Net2HassMqttConfigurationException(message);
        }

        return eventInfo;
    }
}