using NoeticTools.Net2HassMqtt.Exceptions;
using Microsoft.Extensions.Logging;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal sealed class NotSupportedValueConverter(ILogger logger) : ModelValueConverterBase(logger), IModelValueConverter
{
    public bool CanConvert(ModelValueDescriptor valueDescriptor)
    {
        ThrowConfigError($"A {valueDescriptor.HassDomainName} entity requires an int or double status property. {valueDescriptor.ModelPropertyName}'s type is {valueDescriptor.ModelPropertyType.Name}.");
        return true;
    }

    public Func<object?, string> GetConverter(ModelValueDescriptor valueDescriptor)
    {
        ThrowConfigError($"A {valueDescriptor.HassDomainName} entity requires an int or double status property. {valueDescriptor.ModelPropertyName}'s type is {valueDescriptor.ModelPropertyType.Name}.");
        return value => string.Empty;
    }
}