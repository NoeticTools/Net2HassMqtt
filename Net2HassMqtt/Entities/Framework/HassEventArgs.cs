using System.ComponentModel.DataAnnotations;


namespace NoeticTools.Net2HassMqtt.Entities.Framework;

public class HassEventArgs : EventArgs
{
    public HassEventArgs(string eventType) : this(eventType, new Dictionary<string, string>())
    {
    }

    public HassEventArgs(string eventType, IDictionary<string, string> attributes)
    {
        EventType = eventType;
        Attributes = new Dictionary<string, string>(attributes);
    }

    [Required]
    public Dictionary<string, string> Attributes { get; }

    [Required]
    public string EventType { get; }

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