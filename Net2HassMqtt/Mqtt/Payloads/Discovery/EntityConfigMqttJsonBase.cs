using System.Text.Json.Serialization;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

#pragma warning disable IDE1006

namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;

public abstract class EntityConfigMqttJsonBase : IEntityConfigMqttJson
{
    protected internal EntityConfigMqttJsonBase(IEntityConfig config, string entityUniqueId, DeviceConfig deviceConfig, string mqttClientId)
    {
        Device = new DeviceMqttJson(deviceConfig);
        var uniqueId = entityUniqueId.ToMqttTopicSnakeCase();
        UniqueId = uniqueId;
        ObjectId = uniqueId;
        DefaultEntityId = $"{config.MqttTopicComponent}.{uniqueId}";
        Icon = config.Icon;
        Name = config.EntityFriendlyName;
        MqttTopicComponent = config.MqttTopicComponent;
        Availability.Topic = MqttTopic.BuildGatewayStatusTopic(mqttClientId).ToString();
        EntityCategory = config.EntityCategory.HassEntityCategoryName;
        JsonAttributesTopic = StateTopic;
    }

    [JsonPropertyName("availability")]
    public AvailabilityMqttJson Availability { get; } = new();

    [JsonPropertyName("device")]
    public DeviceMqttJson Device { get; set; }

    /// <summary>
    ///     Entity category. Only used for non-primary entities.
    /// </summary>
    [JsonPropertyName("entity_category")]
    public string? EntityCategory { get; set; }

    /// <summary>
    ///     The icon to display in HASS. e.g: mdi:valve.
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("json_attributes_template")]
    public string JsonAttributesTemplate { get; set; } = "{{ value_json.attributes | tojson }}";

    [JsonPropertyName("json_attributes_topic")]
    public string JsonAttributesTopic { get; set; }

    [JsonIgnore]
    public string MqttTopicComponent { get; }

    /// <summary>
    ///     Entity friendly name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The entity ID seen in HASS when referencing this entity.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Home Assistant has deprecated object_id in favor of default_entity_id.
    ///         It will cease to be used Home Assistant after 2024.6.
    ///     </para>
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
    [JsonPropertyName("object_id")]
    public string ObjectId { get; set; } // todo: HASS has depreciated use of object_id in favour of using default_entity_id. No longer used after 2026.4.

    /// <summary>
    ///     The entity ID seen in HASS when referencing this entity. Must be prefixed with the domain, e.g. sensor.foobar.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use default_entity_id instead of name for automatic generation of the entity ID.
    ///         For example, sensor.foobar. When used without a unique_id, the entity ID will update during restart or reload if the entity ID is available. If the entity ID already exists, the entity ID will be created with a number at the end. When used with a unique_id, the default_entity_id is only used when the entity is added for the first time. When set, this overrides a user-customized entity ID if the entity was deleted and added again.
    ///     </para>
    ///     <para>
    ///         For more information see
    ///         <a href="https://www.home-assistant.io/integrations/mqtt/#naming-of-mqtt-entities">
    ///             Home Assistant Naming of Entities
    ///         </a>
    ///         .
    ///     </para>
    /// </remarks>
    [JsonPropertyName("default_entity_id")]
    public string DefaultEntityId { get; set; }

    /// <summary>
    ///     Required. Topic to subscribe to for sensor values.
    /// </summary>
    [JsonPropertyName("state_topic")]
    public string StateTopic { get; set; } = "";

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
    [JsonPropertyName("unique_id")]
    public string UniqueId { get; set; }

    [JsonPropertyName("value_template")]
    public string ValueTemplate { get; set; } = "{{ value_json.state }}";

    internal abstract void Build(TopicBuilder topic);
}