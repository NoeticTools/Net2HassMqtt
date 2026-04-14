using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;

/// <summary>
///     Reader to read a status property value from the model and convert it to an appropriate MQTT state value
///     based on the entity's HASS domain, device class, and unit of measurement.
/// </summary>
internal sealed class StatusPropertyReader : IStatusPropertyReader
{
    private readonly PropertyInfo? _getterPropertyInfo;
    private readonly ILogger _logger;
    private readonly INotifyPropertyChanged _model;
    private readonly string? _propertyName;
    private readonly Func<object?, string> _reader;

    public StatusPropertyReader(INotifyPropertyChanged model,
                                string? statusPropertyName,
                                string? hassDomainName,
                                string? hassDeviceClass,
                                string? hassUoM,
                                IPropertyInfoReader propertyInfoReader,
                                ILogger logger)
    {
        _model = model;
        _propertyName = statusPropertyName;
        _logger = logger;
        if (ModelValueConverters.Count == 0)
        {
            ModelValueConverters =
            [
                new SensorDurationModelValueConverter(logger),
                new SensorTimestampModelValueConverter(logger),
                new SensorEnumModelValueConverter(logger),
                new NumericValueConverter(logger),
                new BoolModelValueConverter(logger),
                new NotSupportedValueConverter(logger)
            ];
        }

        CanRead = !string.IsNullOrWhiteSpace(statusPropertyName);
        if (CanRead)
        {
            var getterPropertyInfo = propertyInfoReader.GetPropertyGetterInfo(model, statusPropertyName);
            if (getterPropertyInfo == null)
            {
                ThrowConfigError($"Unable to find status property {statusPropertyName} on model of type {model.GetType()}.");
            }

            _getterPropertyInfo = getterPropertyInfo;
            _reader = GetReader(hassDomainName, hassDeviceClass, hassUoM);
        }
        else
        {
            _reader = ModelValueConverterBase.DefaultReader;
        }
    }

    /// <summary>
    ///     Can read entity value from the model.
    /// </summary>
    public bool CanRead { get; }

    /// <summary>
    ///     Read value from the model's getter property and convert to appropriate MQTT state value.
    /// </summary>
    public string Read()
    {
        if (string.IsNullOrWhiteSpace(_propertyName))
        {
            throw new InvalidOperationException("Entity is not configured to read model state.");
        }

        var value = _getterPropertyInfo!.GetValue(_model);
        return _reader(value);
    }

    private static List<IModelValueConverter> ModelValueConverters { get; set; } = [];

    private Func<object?, string> GetReader(string? hassDomainName, string? hassDeviceClass, string? hassUoM)
    {
        var propertyValueType = _getterPropertyInfo!.PropertyType;

        if (string.IsNullOrWhiteSpace(hassDomainName))
        {
            return ModelValueConverterBase.DefaultReader;
        }

        var valueDescriptor =
            new ModelValueDescriptor(propertyValueType, hassDomainName, hassDeviceClass, hassUoM, _propertyName!, propertyValueType);
        foreach (var provider in ModelValueConverters)
        {
            if (provider.CanConvert(valueDescriptor))
            {
                return provider.GetConverter(valueDescriptor);
            }
        }

        // should never get here
        return ModelValueConverterBase.DefaultReader;
    }

    [DoesNotReturn]
    private void ThrowConfigError(string message)
    {
        _logger.LogError(message);
        throw new Net2HassMqttConfigurationException(message);
    }
}