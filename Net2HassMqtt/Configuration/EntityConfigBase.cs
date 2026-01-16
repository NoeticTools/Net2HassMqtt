using System.ComponentModel;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Configuration;

public abstract class EntityConfigBase : IEntityConfig
{
    protected EntityConfigBase(HassDomains domain, string? hassDeviceClass)
    {
        Domain = domain;
        MqttTopicComponent = domain.HassDomainName;
        HassDeviceClassName = hassDeviceClass;
        // ReSharper disable once VirtualMemberCallInConstructor
        //CustomInit();
    }

    protected virtual void CustomInit()
    {}

    internal bool CommandHandlerIsRequired { get; init; } = false;

    /// <summary>
    ///     Optional name of the method on <a cref="Model">Model</a> to set the entity's value from received MQTT set.
    ///     One or both of <a cref="StatusPropertyName">GetterPropertyName</a> and
    ///     <a cref="CommandMethodName">GetterPropertyName</a> is required.
    /// </summary>
    public string? CommandMethodName { get; internal set; }

    /// <summary>
    ///     The Home Assistant entity domain (e.g: switch). This is only used for entities (not used by attributes).
    ///     Provides both the HomeAssistant/MQTT snake_case version and the dotnet UpperCamelCase version.
    /// </summary>
    public HassDomains Domain { get; }

    /// <summary>
    ///     Home Assistant device class name.
    /// </summary>
    public string? HassDeviceClassName { get; protected init; }

    internal EntityCategory EntityCategory { get; set; } = EntityCategory.None;

    EntityCategory IEntityConfig.EntityCategory
    {
        get => EntityCategory;
        set => EntityCategory = value;
    }

    /// <summary>
    ///     Name of the property on <a cref="Model"></a> to get the entity's status or attribute value.
    /// </summary>
    protected internal string? StatusPropertyName { get; set; }

    /// <summary>
    ///     Name of the property on <a cref="Model"></a> to get the entity's status or attribute value.
    /// </summary>
    string? IEntityConfig.StatusPropertyName
    {
        get => StatusPropertyName;
        set => StatusPropertyName = value;
    }

    /// <summary>
    /// The name of the model's event member. Only applicable to event entities.
    /// </summary>
    protected internal string? EventMemberName { get; set; }

    /// <summary>
    /// The name of the model's event member. Only applicable to event entities.
    /// </summary>
    string? IEntityConfig.EventMemberName
    {
        get => EventMemberName;
        set => EventMemberName = value;
    }

    /// <summary>
    ///     Optional HASS entity icon.
    /// </summary>
    /// <remarks>
    ///     Home Assistant using material design icons listed <a href="https://pictogrammers.com/library/mdi/">here</a>.
    ///     Naming convention is "mdi:&lt;icon_name&gt;".
    /// </remarks>
    protected internal string? Icon { get; set; }

    /// <summary>
    ///     Optional HASS entity icon.
    /// </summary>
    /// <remarks>
    ///     Home Assistant using material design icons listed <a href="https://pictogrammers.com/library/mdi/">here</a>.
    ///     Naming convention is "mdi:&lt;icon_name&gt;".
    /// </remarks>
    string? IEntityConfig.Icon
    {
        get => Icon;
        set => Icon = value;
    }

    /// <summary>
    ///     The model that has the entity's <a cref="StatusPropertyName">StatusPropertyName</a> property.
    /// </summary>
    internal INotifyPropertyChanged? Model { get; set; }

    /// <summary>
    ///     The model that has the entity's <a cref="StatusPropertyName">StatusPropertyName</a> property.
    /// </summary>
    INotifyPropertyChanged? IEntityConfig.Model
    {
        get => Model;
        set => Model = value;
    }

    /// <summary>
    ///     The entity's Home Assistant domain (and MQTT component topic) such as 'switch' or 'sensor'.
    /// </summary>
    public string MqttTopicComponent { get; internal set; }

    /// <summary>
    ///     Human friendly name for entity name in HASS.
    /// </summary>
    protected internal string EntityFriendlyName { get; set; } = "";

    /// <summary>
    ///     Human friendly name for entity name in HASS.
    /// </summary>
    string IEntityConfig.EntityFriendlyName
    {
        get => EntityFriendlyName;
        set => EntityFriendlyName = value;
    }

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
    public string EntityNodeId { get; private set; } = "";

    public UnitOfMeasurement? UnitOfMeasurement { get; set; }

    List<AttributeConfiguration> IEntityConfig.Attributes { get; } = [];

    void IEntityConfig.SetEntityId(string entityId)
    {
        EntityNodeId = entityId.ToMqttTopicSnakeCase();
    }

    public virtual void Validate()
    {
        if (Model == null)
        {
            throw new Net2HassMqttConfigurationException($"An entity model is required. Type: {GetType().Name}");
        }

        CustomInit();

        var hasGetter = !string.IsNullOrWhiteSpace(StatusPropertyName);
        var hasSetter = !string.IsNullOrWhiteSpace(CommandMethodName);
        var hasEvent = !string.IsNullOrWhiteSpace(EventMemberName);
        if (!hasGetter && !hasSetter && !hasEvent)
        {
            throw new Net2HassMqttConfigurationException("One or more of statusPropertyName, commandMethodName, or eventMemberName is required.");
        }

        if (string.IsNullOrWhiteSpace(EntityNodeId))
        {
            throw new Net2HassMqttConfigurationException($"An EntityId is required. Type: {GetType().Name}");
        }

        if (string.IsNullOrEmpty(EntityFriendlyName))
        {
            throw new Net2HassMqttConfigurationException($"A friendly name is required. Type: {GetType().Name}");
        }

        if (CommandHandlerIsRequired && string.IsNullOrWhiteSpace(CommandMethodName))
        {
            throw new Net2HassMqttConfigurationException($"A CommandMethodName is required for entities of this type. Type: {GetType().Name}");
        }
    }
}