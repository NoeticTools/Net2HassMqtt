using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Entities;

internal abstract class EventEntityBase<T> : EntityBase<T>
    where T : EntityConfigBase
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
        var payload = new EventWithAttributeDataMqttJson(eventArgs.NamedProperties, GetAttributeValuesDictionary());
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

    [DoesNotReturn]
    private void ThrowConfigError(string message)
    {
        Logger.LogError(message);
        throw new Net2HassMqttConfigurationException(message);
    }
}