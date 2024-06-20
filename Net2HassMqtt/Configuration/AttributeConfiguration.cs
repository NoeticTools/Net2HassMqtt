using System.ComponentModel;


namespace NoeticTools.Net2HassMqtt.Configuration;

internal sealed class AttributeConfiguration
{
    public AttributeConfiguration(string name, string? propertyName, string hassUoM, INotifyPropertyChanged model)
    {
        Name = name;
        PropertyName = propertyName;
        HassUnitOfMeasurement = hassUoM;
        Model = model;
    }

    public string HassUnitOfMeasurement { get; }

    public INotifyPropertyChanged Model { get; }

    /// <summary>
    ///     The attribute's name.
    ///     This is displayed in Home Assistant as a friendly name.
    ///     It is also used to reference the attribute, within the entity, in Home Assistant.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The name of the property on the model that holds the attribute's value.
    ///     The property does not need to be notifying.
    ///     Changes to this value do not trigger an entity value update.
    /// </summary>
    public string? PropertyName { get; }
}