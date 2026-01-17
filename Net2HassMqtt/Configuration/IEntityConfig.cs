using System.ComponentModel;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;


namespace NoeticTools.Net2HassMqtt.Configuration;

public interface IEntityConfig
{
    /// <summary>
    ///     Optional name of the method on <a cref="Model">Model</a> to set the entity's value from received MQTT set.
    ///     One or both of <a cref="StatusPropertyName">GetterPropertyName</a> and
    ///     <a cref="CommandMethodName">GetterPropertyName</a> is required.
    /// </summary>
    string? CommandMethodName { get; }

    /// <summary>
    ///     The Home Assistant entity domain (e.g: switch). This is only used for entities (not used by attributes).
    ///     Provides both the HomeAssistant/MQTT snake_case version and the dotnet UpperCamelCase version.
    /// </summary>
    HassDomains Domain { get; }

    /// <summary>
    ///     Home Assistant device class name.
    /// </summary>
    string? HassDeviceClassName { get; }

    EntityCategory EntityCategory { get; protected internal set; }

    /// <summary>
    ///     Name of the property on <a cref="Model"></a> to get the entity's status or attribute value.
    /// </summary>
    string? StatusPropertyName { get; protected internal set; }

    /// <summary>
    /// The name of the model's event member. Only applicable to event entities.
    /// </summary>
    string? EventMemberName { get; protected internal set; }

    /// <summary>
    ///     Optional HASS entity icon.
    /// </summary>
    /// <remarks>
    ///     Home Assistant using material design icons listed <a href="https://pictogrammers.com/library/mdi/">here</a>.
    ///     Naming convention is "mdi:&lt;icon_name&gt;".
    /// </remarks>
    string? Icon { get; protected internal set; }

    /// <summary>
    ///     The model that has the entity's <a cref="StatusPropertyName">StatusPropertyName</a> property.
    /// </summary>
    INotifyPropertyChanged? Model { get; internal set; }

    /// <summary>
    ///     The entity's Home Assistant domain (and MQTT component topic) such as 'switch' or 'sensor'.
    /// </summary>
    string MqttTopicComponent { get; }

    /// <summary>
    ///     Human friendly name for entity name in HASS.
    /// </summary>
    string EntityFriendlyName { get; protected internal set; }

    /// <summary>
    ///     <para>
    ///         The entity's node ID.
    ///         This will be combined with the device's ID to form both unique_id and object_id.
    ///         This must be unique within entities of the same domain on the device
    ///         and must not change.
    ///     </para>
    ///     <para>
    ///         See 'object_id' <a href="https://www.home-assistant.io/integrations/mqtt/">here</a>.
    ///     </para>
    /// </summary>
    string EntityNodeId { get; }

    UnitOfMeasurement? UnitOfMeasurement { get; internal set; }

    internal List<AttributeConfiguration> Attributes { get; }

    internal void SetEntityId(string entityId);

    void Validate();
}