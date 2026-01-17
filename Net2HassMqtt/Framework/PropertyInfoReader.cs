using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Exceptions;


namespace NoeticTools.Net2HassMqtt.Framework;

internal sealed class PropertyInfoReader : IPropertyInfoReader
{
    public PropertyInfo? GetPropertyGetterInfo(object model, string? statusPropertyName)
    {
        if (string.IsNullOrWhiteSpace(statusPropertyName))
        {
            return null;
        }

        var propertyInfo = model.GetType().GetProperty(statusPropertyName, BindingFlags.Instance | BindingFlags.Public);
        if (propertyInfo != null)
        {
            if (!propertyInfo.CanRead)
            {
                ThrowConfigError($"Model property '{statusPropertyName}' must have a getter (be readable).");
            }

            return propertyInfo;
        }

        ThrowConfigError($"Could not find public property '{statusPropertyName}' on model of type of type '{model.GetType()}'");
        return null;
    }

    [DoesNotReturn]
    private static void ThrowConfigError(string message)
    {
        throw new Net2HassMqttConfigurationException(message);
    }
}