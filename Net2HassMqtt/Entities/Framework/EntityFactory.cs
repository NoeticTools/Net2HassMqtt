﻿using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

internal sealed class EntityFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly INet2HassMqttClient _mqttClient;

    public EntityFactory(INet2HassMqttClient mqttClient, ILoggerFactory loggerFactory)
    {
        _mqttClient = mqttClient;
        _loggerFactory = loggerFactory;
    }

    internal IMqttEntity Create(string entityUniqueId, EntityConfigBase entityConfig, DeviceConfig deviceConfig)
    {
        var lookup = new Dictionary<string, Func<EntityConfigBase, string, string, IMqttEntity>>
        {
            { HassDomains.BinarySensor.HassDomainName, CreateBinarySensor },
            { HassDomains.Button.HassDomainName, CreateButton },
            { HassDomains.Cover.HassDomainName, CoverButton },
            { HassDomains.Event.HassDomainName, CreateEvent },
            { HassDomains.Humidifier.HassDomainName, EntityNotSupported }, //todo
            { HassDomains.Number.HassDomainName, CreateNumberEntity },
            { HassDomains.Sensor.HassDomainName, CreateSensorEntity },
            { HassDomains.Switch.HassDomainName, CreateSwitchEntity },
            { HassDomains.Update.HassDomainName, EntityNotSupported }, // todo
            { HassDomains.Valve.HassDomainName, CreateValveEntity }
        };
        entityConfig.Validate();
        return lookup[entityConfig.Domain.HassDomainName](entityConfig, entityUniqueId, deviceConfig.DeviceId.ToMqttTopicSnakeCase());
    }

    private IMqttEntity CoverButton(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new CoverEntity((CoverConfig)config, entityUniqueId, deviceNodeId, _mqttClient, CreateLogger<CoverEntity>(config));
    }

    private IMqttEntity CreateBinarySensor(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new BinarySensorEntity((BinarySensorConfig)config, entityUniqueId, deviceNodeId, _mqttClient,
                                      CreateLogger<BinarySensorEntity>(config));
    }

    private IMqttEntity CreateButton(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new ButtonEntity((ButtonConfig)config, entityUniqueId, deviceNodeId, _mqttClient,
                                CreateLogger<ButtonEntity>(config));
    }

    private IMqttEntity CreateEvent(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new EventEntity((EventConfig)config, entityUniqueId, deviceNodeId, _mqttClient,
                               CreateLogger<EventEntity>(config));
    }

    private ILogger CreateLogger<T>(EntityConfigBase config)
    {
        return _loggerFactory.CreateLogger($"{typeof(T).FullName}({config.EntityFriendlyName})");
    }

    private IMqttEntity CreateNumberEntity(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new NumberEntity((NumberConfig)config, entityUniqueId, deviceNodeId, _mqttClient, 
                                CreateLogger<NumberEntity>(config));
    }

    private IMqttEntity CreateSensorEntity(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new SensorEntity((SensorConfig)config, entityUniqueId, deviceNodeId, _mqttClient, 
                                CreateLogger<SensorEntity>(config));
    }

    private IMqttEntity CreateSwitchEntity(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new SwitchEntity((SwitchConfig)config, entityUniqueId, deviceNodeId, _mqttClient, 
                                CreateLogger<SwitchEntity>(config));
    }

    private IMqttEntity CreateValveEntity(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        return new ValveEntity((ValveConfig)config, entityUniqueId, deviceNodeId, _mqttClient, 
                               CreateLogger<ValveEntity>(config));
    }

    private static IMqttPublisher EntityNotSupported(EntityConfigBase config, string entityUniqueId, string deviceNodeId)
    {
        throw new InvalidOperationException($"Entity domain '{config.Domain.HassDomainName}' not yet implemented.");
    }
}