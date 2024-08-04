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
        _eventHandlerDelegate = Delegate.CreateDelegate(type, this, nameof(OnEvent));
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

    internal void OnEvent(object sender, HassEventArgs eventArgs)
    {
        if (string.IsNullOrWhiteSpace(eventArgs.EventType))
        {
            Logger.LogError("Event is missing required event type. Event not published.");
            return;
        }

        if (!Config.EventTypes.Contains(eventArgs.EventType))
        {
            Logger.LogError($"Event type '{eventArgs.EventType}' is invalid. Valid event types are: {string.Join(", ", Config.EventTypes)}.");
            return;
        }

        var namedProperties = new Dictionary<string, string>(eventArgs.NamedProperties) { { "event_type", eventArgs.EventType } };
        var payload = new EventWithAttributeDataMqttJson(namedProperties, GetAttributeValuesDictionary());
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
        if (eventInfo != null)
        {
            return eventInfo;
        }

        var message = $"Could not find public event '{Config.EventMemberName}.Event' on model of type '{model.GetType()}'";
        Logger.LogError(message);
        throw new Net2HassMqttConfigurationException(message);
    }
}