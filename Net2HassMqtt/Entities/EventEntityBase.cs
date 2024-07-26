using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt.Entities;

// WIP - Refactoring. Structure likely to change considerably.
internal abstract class EventEntityBase<T> : StateEntityBase<T> 
    where T : EntityConfigBase
{
    protected EventEntityBase(T config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger) 
        : base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
    }
}