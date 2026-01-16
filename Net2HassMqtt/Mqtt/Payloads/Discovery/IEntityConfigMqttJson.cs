using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;

public interface IEntityConfigMqttJson
{
    AvailabilityMqttJson Availability { get; }

    DeviceMqttJson Device { get; set; }

    /// <summary>
    ///     Entity category. Only used for non-primary entities.
    /// </summary>
    string? EntityCategory { get; set; }

    /// <summary>
    ///     The icon to display in HASS. e.g: mdi:valve.
    /// </summary>
    string? Icon { get; set; }

    string JsonAttributesTemplate { get; set; }

    string JsonAttributesTopic { get; set; }

    string MqttTopicComponent { get; }

    /// <summary>
    ///     Entity friendly name.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     The entity ID seen in HASS when referencing this entity.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When the entity is created in Home Assistant this value is used to generate the entity's ID.
    ///         As a unique_id is also provided users can change the entity ID in Home Assistant.
    ///         Net2HassMqtt uses the same value for both object_id and unique_id.
    ///     </para>
    ///     <para>
    ///         For more information see
    ///         <a href="https://www.home-assistant.io/integrations/mqtt/#naming-of-mqtt-entities">
    ///             Home Assistant Naming of Entities
    ///         </a>
    ///         .
    ///     </para>
    /// </remarks>
    string ObjectId { get; set; }

    /// <summary>
    ///     Required. Topic to subscribe to for sensor values.
    /// </summary>
    string StateTopic { get; set; }

    /// <summary>
    ///     A unique ID that should never change.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Providing unique_id allows for the entity ID generated from object_id to be changed by the user.
    ///         Net2HassMqtt uses the same value for both object_id and unique_id.
    ///     </para>
    ///     <para>
    ///         For more information see
    ///         <a href="https://www.home-assistant.io/integrations/mqtt/#naming-of-mqtt-entities">
    ///             Home Assistant Naming of
    ///             Entities
    ///         </a>
    ///         .
    ///     </para>
    /// </remarks>
    string UniqueId { get; set; }

    string ValueTemplate { get; set; }
}