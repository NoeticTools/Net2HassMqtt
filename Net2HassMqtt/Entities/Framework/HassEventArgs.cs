using System.ComponentModel.DataAnnotations;
using System.Drawing;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

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
    public static HassEventArgs From<T>(T eventTypeId)
        where T : Enum
    {
        var name = Enum.GetName(typeof(T), eventTypeId);
        return new HassEventArgs(name!);
    }

    public static HassEventArgs From<T>(T eventTypeId, Dictionary<string, string> attributes)
        where T : Enum
    {
        var name = Enum.GetName(typeof(T), eventTypeId);
        return new HassEventArgs(name!, attributes);
    }
}
