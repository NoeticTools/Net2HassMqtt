namespace NoeticTools.Net2HassMqtt.Exceptions;

public sealed class Net2HassMqttInvalidTopicException : Net2HassMqttException
{
    public Net2HassMqttInvalidTopicException(string message)
        : base(message)
    {
    }
}