namespace NoeticTools.Net2HassMqtt.Exceptions;

public abstract class Net2HassMqttException : Exception
{
    protected Net2HassMqttException(string message)
        : base(message)
    {
    }
}