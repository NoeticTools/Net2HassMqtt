using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.Extensions.ManagedClient;
using NoeticTools.Net2HassMqtt.Entities;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using Serilog;


namespace NoeticTools.Net2HassMqtt.Configuration.Building;

internal static class HassMqttBridgeBuilder
{
    public static INet2HassMqttBridge Build(BridgeConfiguration config, IManagedMqttClient? client = null)
    {
        config.Validate();

        var serviceProvider = GetServiceProvider(config, client);
        var m2HClient = serviceProvider.GetService<INet2HassMqttClient>()!;
        var deviceFactory = serviceProvider.GetService<DeviceFactory>()!;
        var entityModelFactory = serviceProvider.GetService<EntityFactory>()!;

        var devices = new List<Device>();
        foreach (DeviceConfig deviceConfig in config.Devices)
        {
            var device = deviceFactory.Create(deviceConfig);
            AddEntities(device, deviceConfig, entityModelFactory);
            devices.Add(device);
        }

        var loggerFactory = serviceProvider.GetService<ILoggerFactory>()!;
        return new Net2MqttBridge(m2HClient, devices, loggerFactory.CreateLogger<Net2MqttBridge>());
    }

    private static void AddEntities(Device device, DeviceConfig deviceConfig, EntityFactory entityFactory)
    {
        foreach (var (entityNodeId, config) in deviceConfig.Entities)
        {
            var entityConfig = (EntityConfigBase)config;
            var entityUniqueId = deviceConfig.BuildEntityUniqueId(entityNodeId);
            var entity = entityFactory.Create(entityUniqueId, entityConfig, deviceConfig);
            device.AddEntity(entityNodeId, entity);
        }
    }

    private static IServiceProvider GetServiceProvider(BridgeConfiguration config, IManagedMqttClient? client)
    {
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(config);
            services.AddSingleton(config.MqttOptions!);
            services.AddSingleton<HassMqttClientFactory>();
            services.AddSingleton(x =>
            {
                var factory = x.GetService<HassMqttClientFactory>()!;
                return factory.Create(x.GetService<ManagedMqttClientOptions>()!, client);
            });
            services.AddTransient<EntityFactory>();
            services.AddSingleton<DeviceFactory>();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true)
                                                                .AddConsole()
                                                                .SetMinimumLevel(LogLevel.Debug));
        });
        return builder.Build().Services;
    }
}