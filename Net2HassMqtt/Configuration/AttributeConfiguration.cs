using System.ComponentModel;


namespace NoeticTools.Net2HassMqtt.Configuration;

internal sealed class AttributeConfiguration(string name, string? propertyName, string hassUoM, INotifyPropertyChanged model)
{
    public string HassUnitOfMeasurement { get; } = hassUoM;

    public INotifyPropertyChanged Model { get; } = model;

    /// <summary>
    ///     The attribute's name.
    ///     This is displayed in Home Assistant as a friendly name.
    ///     It is also used to reference the attribute, within the entity, in Home Assistant.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     The name of the property on the model that holds the attribute's value.
    ///     The property does not need to be notifying.
    ///     Changes to this value do not trigger an entity value update.
    /// </summary>
    public string? PropertyName { get; } = propertyName;
}