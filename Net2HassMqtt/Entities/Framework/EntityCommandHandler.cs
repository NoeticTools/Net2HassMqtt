using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Exceptions;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

internal sealed class EntityCommandHandler
{
    private readonly MethodInfo? _commandMethodInfo;
    private readonly string? _commandMethodName;
    private readonly Type? _commandMethodParameterValueType;
    private readonly string? _hassUoM;
    private readonly ILogger _logger;
    private readonly INotifyPropertyChanged _model;

    public EntityCommandHandler(INotifyPropertyChanged model, string? commandMethodName, string? hassUoM, ILogger logger)
    {
        _model = model;
        _commandMethodName = commandMethodName;
        _hassUoM = hassUoM;
        _logger = logger;
        _commandMethodInfo = GetCommandMethodInfo(commandMethodName);
        _commandMethodParameterValueType = _commandMethodInfo?.GetParameters()[0].ParameterType;
        CanCommand = _commandMethodInfo != null;
    }

    public bool CanCommand { get; }

    public void Handle(string mqttPayload)
    {
        if (!CanCommand)
        {
            _logger.LogWarning("Cannot handle received MQTT command as commands are not configured on this entity.");
            return;
        }

        var value = new MqttCommandPayloadReader(mqttPayload, _hassUoM, _logger).ConvertTo(_commandMethodParameterValueType!);
        InvokeCommandMethod(value);
    }

    private MethodInfo? GetCommandMethodInfo(string? commandMethodName)
    {
        if (string.IsNullOrWhiteSpace(commandMethodName))
        {
            return null;
        }

        var methodInfo = _model.GetType().GetMethod(commandMethodName, BindingFlags.Instance | BindingFlags.Public);
        if (methodInfo != null)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length != 1)
            {
                var message = $"Model setter method {commandMethodName} on {_model.GetType()} must have one argument.";
                _logger.LogError(message);
                throw new Net2HassMqttConfigurationException(message);
            }

            return methodInfo!;
        }

        {
            var message = $"Could not find public method '{commandMethodName}' on model of type of type '{_model.GetType()}'";
            _logger.LogError(message);
            throw new Net2HassMqttConfigurationException(message);
        }
    }

    private void InvokeCommandMethod(object? value)
    {
        if (_commandMethodInfo == null)
        {
            _logger.LogWarning("The entity is configured as read only as no setter method was given. Unable to set model to MQTT value received");
            return;
        }

        try
        {
            _commandMethodInfo!.Invoke(_model, new[] { value });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception when invoking method setter method {0}.", _commandMethodName);
        }
    }
}