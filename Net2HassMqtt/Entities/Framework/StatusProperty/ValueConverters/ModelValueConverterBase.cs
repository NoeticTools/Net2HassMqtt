using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Exceptions;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal abstract class ModelValueConverterBase(ILogger logger)
{
    protected static string DefaultReader(object? arg)
    {
        if (arg == null)
        {
            return "null";
        }

        return arg.ToString() ?? "null";
    }

    [DoesNotReturn]
    protected void ThrowConfigError(string message)
    {
        logger.LogError(message);
        throw new Net2HassMqttConfigurationException(message);
    }

    [DoesNotReturn]
    protected void ThrowUnableToFindReaderException(ModelValueDescriptor valueDescriptor)
    {
        ThrowConfigError($"""
                          Unable to find reader for model property {valueDescriptor.ModelPropertyName} of type {valueDescriptor.ModelPropertyType.Name}.
                          No match for entity domain {valueDescriptor.HassDomainName}, device class {valueDescriptor.HassDeviceClass}.");
                          """);
    }
}