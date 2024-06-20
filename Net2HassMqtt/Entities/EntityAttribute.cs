using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;


namespace NoeticTools.Net2HassMqtt.Entities;

/// <summary>
///     An attribute on a Home Assistant entity. Each entity may have 0 or more attributes.
/// </summary>
internal sealed class EntityAttribute : EntityPropertyBase
{
    public EntityAttribute(AttributeConfiguration config, ILogger logger)
        : base(config, logger)

    {
        Name = config.Name;
    }

    /// <summary>
    ///     The attribute's name. This name is used in Home Assistant to view and access the attribute.
    /// </summary>
    public string Name { get; }
}