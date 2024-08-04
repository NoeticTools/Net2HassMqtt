using System.ComponentModel.DataAnnotations;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;


public class HassEventArgs : EventArgs
{
    [Required]
    public string EventType { get; }

    [Required]
    public Dictionary<string, string> NamedProperties { get; }

    public HassEventArgs(string eventType) : this(eventType,  new Dictionary<string, string>())
    {
    }

    public HassEventArgs(string eventType, IDictionary<string, string> args)
    {
        EventType = eventType;
        NamedProperties = new Dictionary<string, string>(args);
    }
}
