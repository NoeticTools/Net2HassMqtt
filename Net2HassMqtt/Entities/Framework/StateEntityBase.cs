using System.ComponentModel;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.State;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


// ReSharper disable MemberCanBePrivate.Global

namespace NoeticTools.Net2HassMqtt.Entities.Framework;

/// <summary>
///     State entity base class.
/// </summary>
internal abstract class StateEntityBase<T> : EntityBase<T>, IMqttPublisher, IMqttSubscriber
    where T : EntityConfigBase
{
    protected StateEntityBase(T config, string entityUniqueId, string deviceNodeId,
                              INet2HassMqttClient mqttClient, IPropertyInfoReader propertyInfoReader, ILogger logger)
        : base(config, entityUniqueId, deviceNodeId, mqttClient, propertyInfoReader, logger)
    {
        StatusPropertyReader = new StatusPropertyReader(config.Model!,
                                                        config.StatusPropertyName,
                                                        config.Domain.HassDomainName,
                                                        config.HassDeviceClassName,
                                                        config.UnitOfMeasurement!.HassUnitOfMeasurement,
                                                        propertyInfoReader,
                                                        logger);

        CommandHandler = new EntityCommandHandler(config.Model!,
                                                  config.CommandMethodName,
                                                  config.UnitOfMeasurement!.HassUnitOfMeasurement,
                                                  logger);
        CanCommand = CommandHandler.CanCommand;
    }

    public EntityCommandHandler CommandHandler { get; set; }

    public IStatusPropertyReader StatusPropertyReader { get; }

    public async Task PublishStateAsync()
    {
        var payload = new StateWithAttributeDataMqttJson(StatusPropertyReader.Read(), GetAttributeValuesDictionary());
        await PublishStatusAsync(payload);
    }

    public override Task StartAsync()
    {
        Config.Model!.PropertyChanged += OnModelPropertyChanged;
        return Task.CompletedTask;
    }

    public override Task StopAsync()
    {
        Config.Model!.PropertyChanged -= OnModelPropertyChanged;
        return Task.CompletedTask;
    }

    private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Config.StatusPropertyName)
        {
            // ReSharper disable once UseDiscardAssignment
            var _ = PublishStateAsync();
        }
    }

    void IMqttSubscriber.OnReceived(ReceivedMqttMessage message)
    {
        if (message.TopicAction == TopicAction.SetCmd)
        {
            CommandHandler.Handle(message.Payload);
        }
    }
}