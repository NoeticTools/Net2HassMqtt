using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;


// ReSharper disable ConvertToPrimaryConstructor

namespace NoeticTools.Net2HassMqtt.Entities;

/// <summary>
///     An attribute on a Home Assistant entity. Each entity may have 0 or more attributes.
/// </summary>
internal sealed class EntityAttribute
{
    public EntityAttribute(AttributeConfiguration config, ILogger logger)

    {
        Name = config.Name;
        Logger = logger;
        StatusPropertyReader = new StatusPropertyReader(config.Model, config.PropertyName, null, null, config.HassUnitOfMeasurement, logger);
    }

    /// <summary>
    ///     The attribute's name. This name is used in Home Assistant to view and access the attribute.
    /// </summary>
    public string Name { get; }

    public IStatusPropertyReader StatusPropertyReader { get; }

    protected ILogger Logger { get; }
}