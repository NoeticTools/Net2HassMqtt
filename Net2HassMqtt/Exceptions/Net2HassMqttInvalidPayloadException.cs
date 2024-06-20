namespace NoeticTools.Net2HassMqtt.Exceptions;

public sealed class Net2HassMqttInvalidPayloadException : Net2HassMqttException
{
    public Net2HassMqttInvalidPayloadException(string message)
        : base(message)
    {
    }
}