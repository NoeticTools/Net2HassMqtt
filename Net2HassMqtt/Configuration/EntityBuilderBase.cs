using System.ComponentModel;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Exceptions;


namespace NoeticTools.Net2HassMqtt.Configuration;

public abstract class EntityBuilderBase<T, TC>
    where T : EntityBuilderBase<T, TC>
    where TC : class, IEntityConfig
{
    protected internal EntityBuilderBase(TC entityConfig)
    {
        EntityConfig = entityConfig;
    }

    public TC EntityConfig { get; }

    public T InCategory(EntityCategory category)
    {
        EntityConfig.EntityCategory = category;
        return (this as T)!;
    }

    /// <summary>
    ///     The model that will be observed for changes.
    ///     Must implement <see cref="INotifyPropertyChanged" />.
    ///     The entity will read the initial value from the model and then update whenever the model raises a property change
    ///     notification for the property specified by <see cref="WithStatusProperty(string)" />. If
    ///     <see cref="WithEvent(string)" /> is used, the entity will also listen for changes to the model's event member
    ///     specified by <see cref="WithEvent(string)" />. The model must not be null. The model must not change after being
    ///     set here. If you need to change the model, you must create a new entity configuration and builder.
    /// </summary>
    /// <param name="model">The model that will be observed for changes.</param>
    /// <returns>The builder instance.</returns>
    public T OnModel(INotifyPropertyChanged model)
    {
        EntityConfig.Model = model;
        return (this as T)!;
    }

    public T WithAttribute(string? propertyName, string name, string unitOfMeasure = "")
    {
        EntityConfig.Attributes.Add(new AttributeConfiguration(name, propertyName, unitOfMeasure, EntityConfig.Model!));
        return (this as T)!;
    }

    // todo - generate attribute methods with units of measurement && DurationAttributeUoM

    public T WithDurationAttribute(string? propertyName, string name, DurationSensorUoM unitOfMeasure)
    {
        return WithAttribute(propertyName, name, unitOfMeasure.HassUnitOfMeasurement);
    }

    /// <summary>
    ///     The name of the model's event member. Only applicable to event entities.
    /// </summary>
    public T WithEvent(string haEventMemberName)
    {
        EntityConfig.EventMemberName = haEventMemberName;

        var model = EntityConfig.Model;
        if (model == null)
        {
            throw new Net2HassMqttConfigurationException("WithEvent requires a model");
        }

        return (this as T)!;
    }

    /// <summary>
    ///     The entity name that will be seen in Home Assistant.
    /// </summary>
    public T WithFriendlyName(string friendlyName)
    {
        EntityConfig.EntityFriendlyName = friendlyName;
        return (this as T)!;
    }

    public T WithHumidityAttribute(string? propertyName, string name, HumiditySensorUoM unitOfMeasure)
    {
        return WithAttribute(propertyName, name, unitOfMeasure.HassUnitOfMeasurement);
    }

    public T WithIcon(string name)
    {
        EntityConfig.Icon = name;
        return (this as T)!;
    }

    /// <summary>
    ///     <para>
    ///         The node ID is required.
    ///         The node must be unique within the entity domain on the current device.
    ///         This ID must not change.
    ///     </para>
    ///     <para>It will be combined with the device's ID to form the entity ID.</para>
    /// </summary>
    public T WithNodeId(string nodeId)
    {
        EntityConfig.SetEntityId(nodeId);
        return (this as T)!;
    }

    /// <summary>
    ///     The name of the notifying property on the model that provides the entity status value.
    ///     Use <c>nameof(&lt;model&gt;.&lt;statusProperty&gt;)</c>.
    /// </summary>
    public T WithStatusProperty(string propertyName)
    {
        EntityConfig.StatusPropertyName = propertyName;
        return (this as T)!;
    }

    public T WithTemperatureAttribute(string? propertyName, string name, TemperatureSensorUoM unitOfMeasure)
    {
        return WithAttribute(propertyName, name, unitOfMeasure.HassUnitOfMeasurement);
    }
}