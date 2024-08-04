using System.ComponentModel.DataAnnotations;
using System.Drawing;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;


public class HassEventArgs<T> : EventArgs
    where T : Enum
{
    [Required]
    public int EventTypeId { get; }

    [Required]
    public Dictionary<string, string> Attributes { get; }

    public HassEventArgs(T eventTypeId) : this(eventTypeId, new Dictionary<string, string>())
    {
    }

    public HassEventArgs(T eventTypeId, IDictionary<string, string> attributes)
    {
        EventTypeId = Convert.ToInt32(eventTypeId);
        Attributes = new Dictionary<string, string>(attributes);
    }
}

public class HassEventArgs : EventArgs
{
    [Required]
    public string EventType { get; }

    [Required]
    public Dictionary<string, string> Attributes { get; }

    public HassEventArgs(string eventType) : this(eventType,  new Dictionary<string, string>())
    {
    }

    public HassEventArgs(string eventType, IDictionary<string, string> attributes)
    {
        EventType = eventType;
        Attributes = new Dictionary<string, string>(attributes);
    }
}
