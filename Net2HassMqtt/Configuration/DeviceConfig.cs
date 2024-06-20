// ReSharper disable CollectionNeverQueried.Global
// ReSharper disable MemberCanBePrivate.Global

using NoeticTools.Net2HassMqtt.Configuration.Building;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt.Configuration;

public sealed class DeviceConfig : IEntityUniqueIdBuilder
{
    internal DeviceConfig()
    {
    }

    /// <summary>
    ///     The Home Assistant MQTT client ID that is used for HASS discovery.
    ///     Entity configurations are published to this MQTT client.
    /// </summary>
    public string HomeAssistantClientId { get; set; } = MqttConstants.DefaultHassMqttClientId;

    /// <summary>
    ///     A unique IDs that uniquely identifies this device on the MQTT broker and in Home Assistant.
    ///     This ID must not change.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The first ID in this array is taken as the device ID which is used as a prefix for all entity
    ///         Home Assistant unique_id and initial object_id values.
    ///     </para>
    ///     <para>
    ///         This ID is visible in Home Assistant as entity ID prefix.
    ///         This ID must not change between application shutdown and restart.
    ///         If it does change (e.g: after restarting an application) duplicate devices will appear in Home Assistant.
    ///     </para>
    ///     <para>
    ///         Convention is to use a string like: "0xa085cb88e7124da89".
    ///         However, a more readable ID like "my_widget_device_1" is recommended as makes debugging easier and gives better
    ///         initial entity object_ids.
    ///     </para>
    /// </remarks>
    internal string[] Identifiers { get; set; } = Array.Empty<string>();

    /// <summary>
    ///     Optional device manufacturer's name. Displayed in Home Assistant.
    /// </summary>
    internal string Manufacturer { get; set; } = "";

    /// <summary>
    ///     <para>
    ///         Optional manufacturer's device model. Displayed in Home Assistant.
    ///     </para>
    /// </summary>
    internal string Model { get; set; } = "";

    /// <summary>
    ///     <para>
    ///         Required descriptive device name.
    ///         A device with this name will be created in Home Assistant.
    ///     </para>
    ///     <para>
    ///         The device name can be changed in Home Assistant or Net2HassMqtt.
    ///     </para>
    /// </summary>
    internal string Name { get; set; } = "";

    internal string DeviceId => Identifiers[0];

    /// <summary>
    ///     <para>
    ///         This device's entities. A device must have at least one entity.
    ///     </para>
    /// </summary>
    internal Dictionary<string, object> Entities { get; set; } = new();

    public string BuildEntityUniqueId(string entityNodeId)
    {
        if (Identifiers.Length == 0 || string.IsNullOrWhiteSpace(DeviceId))
        {
            throw new Net2HassMqttConfigurationException("The device's ID is required prior to building entity IDs.");
        }

        return $"{DeviceId}_{entityNodeId}";
    }

    /// <summary>
    ///     <para>
    ///         Home Assistant MQTT client ID.
    ///         Only required if using a non-default client ID.
    ///         Used by Home Assistant MQTT discovery.
    ///     </para>
    ///     <para>
    ///         The default client ID is "homeassistant".
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     This is typically only required if HASS was set up with a different MQTT client ID.
    /// </remarks>
    public DeviceConfig OnHomeAssistantMqttClient(string mqttClientId)
    {
        HomeAssistantClientId = mqttClientId;
        return this;
    }

    /// <summary>
    ///     Set optional device manufacturer's name. Displayed in Home Assistant.
    /// </summary>
    public DeviceConfig WithManufacturer(string manufacturer)
    {
        Manufacturer = manufacturer;
        return this;
    }

    /// <summary>
    ///     Set optional device model. Displayed in Home Assistant as the manufacturer's device model number or name.
    /// </summary>
    public DeviceConfig WithModelName(string model)
    {
        Model = model;
        return this;
    }

    internal void AddEntity(string entityId, EntityConfigBase config)
    {
        if (!Entities.TryAdd(entityId, config))
        {
            throw new Net2HassMqttConfigurationException($"All entities on a device must have a unique node ID. '{entityId}' is not unique.");
        }
    }

    internal IReadOnlyList<string> Validate()
    {
        var errors = new List<string>();

        if (!Identifiers.Any())
        {
            errors.Add("A device requires an ID (Device ID).");
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            errors.Add("A device requires a name.");
        }

        if (Entities.Count == 0)
        {
            errors.Add("A device requires a one or more entities.");
        }

        return errors;
    }
}