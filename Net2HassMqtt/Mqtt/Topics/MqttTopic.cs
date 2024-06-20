namespace NoeticTools.Net2HassMqtt.Mqtt.Topics;

/// <summary>
///     An MQTT topic.
/// </summary>
/// <remarks>
///     <para>
///         A topic is a forward slash delimited string that defines what the messages purpose. Used by clients to
///         subscribe to
///         messages.
///     </para>
///     <para>See <seealso href="https://www.hivemq.com/blog/mqtt-essentials-part-5-mqtt-topics-best-practices/" /></para>
/// </remarks>
internal sealed class MqttTopic
{
    private readonly string _topic;

    private MqttTopic(string topic)
    {
        _topic = topic.ToMqttTopicSnakeCase();
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(_topic);

    public static MqttTopic BuildGatewayStatusTopic(string clientId)
    {
        var topic = new MqttTopic(clientId);
        return topic! / MqttConstants.WillSubTopic;
    }

    public static MqttTopic operator /(MqttTopic lhs, TopicAction rhs)
    {
        return new MqttTopic($"{lhs._topic}/{rhs.ToTopicElement()}");
    }

    public static MqttTopic operator /(MqttTopic lhs, string rhs)
    {
        return string.IsNullOrWhiteSpace(rhs) ? lhs : new MqttTopic($"{lhs._topic}/{rhs}");
    }

    public static MqttTopic operator /(string lhs, MqttTopic rhs)
    {
        return new MqttTopic($"{lhs}/{rhs._topic}");
    }

    public static MqttTopic operator /(MqttTopic lhs, MqttTopic rhs)
    {
        return new MqttTopic($"{lhs._topic}/{rhs._topic}");
    }

    public static implicit operator MqttTopic(string topic)
    {
        return new MqttTopic(topic);
    }

    public override string ToString()
    {
        return _topic;
    }
}