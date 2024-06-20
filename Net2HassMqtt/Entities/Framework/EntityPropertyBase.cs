using System.ComponentModel;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

internal abstract class EntityPropertyBase
{
    protected EntityPropertyBase(AttributeConfiguration config, ILogger logger)
        : this(config.PropertyName, null, null, config.HassUnitOfMeasurement, config.Model, logger)
    {
    }

    protected EntityPropertyBase(EntityConfigBase config, ILogger logger)
        : this(config.StatusPropertyName, config.Domain.HassDomainName, config.HassDeviceClassName, config.UnitOfMeasurement!.HassUnitOfMeasurement,
               config.Model!, logger)
    {
    }

    private EntityPropertyBase(string? statusPropertyName,
                               string? hassDomainName, string? hassDeviceClass, string hassUoM,
                               INotifyPropertyChanged model,
                               ILogger logger)
    {
        Model = model;
        StatusPropertyReader = new StatusPropertyReader(model, statusPropertyName, hassDomainName, hassDeviceClass, hassUoM, logger);
        Logger = logger;
    }

    public IStatusPropertyReader StatusPropertyReader { get; }

    protected ILogger Logger { get; }

    protected INotifyPropertyChanged Model { get; }
}