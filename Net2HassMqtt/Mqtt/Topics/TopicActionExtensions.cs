namespace NoeticTools.Net2HassMqtt.Mqtt.Topics;

internal static class TopicActionExtensions
{
    private static readonly Dictionary<TopicAction, string> LookupByAction;
    private static readonly Dictionary<string, TopicAction> LookupByName;

    static TopicActionExtensions()
    {
        LookupByAction = new Dictionary<TopicAction, string>
        {
            { TopicAction.Config, "config" },
            { TopicAction.SetCmd, "set" },
            { TopicAction.Status, "status" }
        };

        LookupByName = new Dictionary<string, TopicAction>();
        foreach (var (action, name) in LookupByAction)
        {
            LookupByName.Add(name, action);
        }
    }

    public static TopicAction ToTopicAction(this string name)
    {
        return LookupByName[name.ToLower()];
    }

    public static MqttTopic ToTopicElement(this TopicAction action)
    {
        return LookupByAction[action];
    }
}