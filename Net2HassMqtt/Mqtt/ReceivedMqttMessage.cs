using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Mqtt;

internal sealed class ReceivedMqttMessage
{
    public ReceivedMqttMessage(TopicBuilder topicBuilder, string payload)
    {
        Payload = payload;
        BaseTopic = topicBuilder.BaseTopic.ToString();
        Component = topicBuilder.Component;
        NodeId = topicBuilder.NodeId.ToString();
        ObjectId = topicBuilder.ObjectId.ToString();
        TopicAction = topicBuilder.TopicAction;
        IsValid = topicBuilder.IsValidNet2HassMessage;
    }

    public string BaseTopic { get; }

    public string Component { get; }

    public bool IsValid { get; }

    public string NodeId { get; }

    public string ObjectId { get; }

    public string Payload { get; }

    public TopicAction TopicAction { get; }
}