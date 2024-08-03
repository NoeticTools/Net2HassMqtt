namespace NoeticTools.Net2HassMqtt.Entities.Framework;


public class HassEventArgs : EventArgs
{
    public Dictionary<string, string> Arguments { get; }

    public HassEventArgs(string eventType) : this(eventType,  new Dictionary<string, string>())
    {
    }

    public HassEventArgs(string eventType, IDictionary<string, string> args)
    {
        Arguments = new Dictionary<string, string>(args) { { "event_type", eventType } };
    }
}
