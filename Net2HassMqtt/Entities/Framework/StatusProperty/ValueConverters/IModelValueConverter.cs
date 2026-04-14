namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal interface IModelValueConverter
{
    /// <summary>
    ///     Determines whether the specified value descriptor can be converted by this converter.
    /// </summary>
    /// <param name="valueDescriptor">
    ///     The descriptor that provides information about the model value to evaluate for conversion capability. Cannot be
    ///     null.
    /// </param>
    /// <returns>true if the value described by valueDescriptor can be converted; otherwise, false.</returns>
    bool CanConvert(ModelValueDescriptor valueDescriptor);

    Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor);
}