namespace NoeticTools.Net2HassMqtt.Entities.Framework;

public static class HassEventArgsFactory
{
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