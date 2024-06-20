using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt;


namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;

internal sealed class StatusPropertyReader : IStatusPropertyReader
{
    private static readonly HashSet<string> ValueHassDomains =
    [
        HassDomains.Humidifier.HassDomainName,
        HassDomains.Sensor.HassDomainName,
        HassDomains.Number.HassDomainName
    ];

    private static readonly Dictionary<string, Func<object?, string>> BooleanReadersByHassDomains = new()
    {
        { HassDomains.BinarySensor.HassDomainName, ToOnOffValue },
        { HassDomains.Switch.HassDomainName, ToOnOffValue },
        { HassDomains.Valve.HassDomainName, ToOpenClosedValue },
        { HassDomains.Cover.HassDomainName, ToOpenClosedCover }
    };

    private static readonly Dictionary<string, Func<TimeSpan, double>> TimeSpanReadersByUoM = new()
    {
        { HassUoMs.Seconds, span => span.TotalSeconds },
        { HassUoMs.Minutes, span => span.TotalMinutes },
        { HassUoMs.Hours, span => span.TotalHours },
        { HassUoMs.Days, span => span.TotalDays }
    };

    private readonly PropertyInfo? _getterPropertyInfo;
    private readonly string? _getterPropertyName;
    private readonly ILogger _logger;
    private readonly INotifyPropertyChanged _model;
    private readonly Func<object?, string> _reader;

    public StatusPropertyReader(INotifyPropertyChanged model, string? statusPropertyName,
                                string? hassDomainName, string? hassDeviceClass,
                                string hassUoM, ILogger logger)
    {
        _model = model;
        _getterPropertyName = statusPropertyName;
        _logger = logger;
        CanRead = !string.IsNullOrWhiteSpace(statusPropertyName);
        if (CanRead)
        {
            var getterPropertyInfo = GetGetterPropertyInfo(statusPropertyName);
            if (getterPropertyInfo == null)
            {
                ThrowConfigError($"Unable to find status property {statusPropertyName} on model of type {model.GetType()}.");
            }

            _getterPropertyInfo = getterPropertyInfo;
            _reader = GetReader(hassDomainName, hassDeviceClass, hassUoM);
        }
        else
        {
            _reader = DefaultReader;
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
        if (string.IsNullOrWhiteSpace(_getterPropertyName))
        {
            throw new InvalidOperationException("Entity is not configured to read model state.");
        }

        var value = _getterPropertyInfo!.GetValue(_model);

        return _reader(value);
    }

    private static string ToOnOffValue(object? value)
    {
        return (bool)value! ? MqttConstants.EntityOnState : MqttConstants.EntityOffState;
    }

    private static string ToOpenClosedCover(object? cover)
    {
        return (bool)cover! ? MqttConstants.EntityOpenState : MqttConstants.EntityClosedState;
    }

    private static string ToOpenClosedValue(object? value)
    {
        return (bool)value! ? MqttConstants.EntityOpenState : MqttConstants.EntityClosedState;
    }

    private static string DefaultReader(object? arg)
    {
        if (arg == null)
        {
            return "null";
        }

        return arg.ToString() ?? "null";
    }

    private Func<object?, string> GetReader(string? hassDomainName, string? hassDeviceClass, string hassUoM)
    {
        var propertyValueType = _getterPropertyInfo!.PropertyType;

        if (string.IsNullOrWhiteSpace(hassDomainName))
        {
            return DefaultReader;
        }

        if (hassDomainName == HassDomains.Sensor.HassDomainName &&
            hassDeviceClass == SensorDeviceClass.Duration.HassDeviceClassName &&
            propertyValueType == typeof(TimeSpan))
        {
            if (TimeSpanReadersByUoM.TryGetValue(hassUoM, out var timespanReader))
            {
                return value =>
                {
                    var result = timespanReader((TimeSpan)value!);
                    return DefaultReader(result);
                };
            }

            return value =>
            {
                var result = ((TimeSpan)value!).TotalMinutes.ToString(CultureInfo.CurrentCulture);
                return DefaultReader(result);
            };
        }

        if (ValueHassDomains.Contains(hassDomainName))
        {
            if (propertyValueType == typeof(int) || propertyValueType == typeof(double))
            {
                return DefaultReader;
            }

            ThrowConfigError($"A {hassDomainName} entity requires an int or double status property. {_getterPropertyName}'s type is {propertyValueType}.");
        }

        if (BooleanReadersByHassDomains.TryGetValue(hassDomainName, out var reader))
        {
            if (propertyValueType != typeof(bool))
            {
                ThrowConfigError($"A {hassDomainName} entity requires a bool status property. {_getterPropertyName}'s type is {propertyValueType}.");
            }

            return reader;
        }

        _logger.LogWarning($"""
                            Unable to find status property reader for property {_getterPropertyName} on model of type {_model.GetType()}.
                            No match for entity domain {hassDomainName}, device class {hassDeviceClass}, and status property type {propertyValueType}.");
                            """);

        return DefaultReader;
    }

    [DoesNotReturn]
    private void ThrowConfigError(string message)
    {
        _logger.LogError(message);
        throw new Net2HassMqttConfigurationException(message);
    }

    private PropertyInfo? GetGetterPropertyInfo(string? statusPropertyName)
    {
        if (string.IsNullOrWhiteSpace(statusPropertyName))
        {
            return null;
        }

        var propertyInfo = _model.GetType().GetProperty(statusPropertyName, BindingFlags.Instance | BindingFlags.Public);
        if (propertyInfo != null)
        {
            if (!propertyInfo.CanRead)
            {
                _logger.LogError("Model property '{0}' must have a getter (be readable).", statusPropertyName);
            }

            return propertyInfo;
        }

        ThrowConfigError($"Could not find public property '{statusPropertyName}' on model of type of type '{_model.GetType()}'");
        return null;
    }
}