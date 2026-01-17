using NoeticTools.Net2HassMqtt.Exceptions;


namespace NoeticTools.Net2HassMqtt.Mqtt.Topics;

/// <summary>
///     Build a topic with the format: &lt;base topic&gt;/&lt;component&gt;/&lt;node_id&gt;/&lt;object_id&gt;/&lt;action
///     &gt;
/// </summary>
//  <base topic>/<component>/<node_id>/<object_id>/<action>
internal sealed class TopicBuilder
{
    public TopicBuilder()
    {
    }

    private TopicBuilder(TopicBuilder other) : this()
    {
        Component = other.Component;
        NodeId = other.NodeId;
        ObjectId = other.ObjectId;
        TopicAction = other.TopicAction;
        BaseTopic = other.BaseTopic;
    }

    private TopicBuilder(string topic)
    {
        try
        {
            var elements = topic.Split('/');
            if (elements.Length != 4)
            {
                Console.Error.WriteLine($"Received MQTT topic '{topic}' is not valid.");
                return;
            }

            // Home assistants topic convention for discovery is:
            //
            //      BaseTopic / Component / NodeId / ObjectId / TopicAction
            //
            // For easier debugging, Net2HassMqtt groups status and commands by NodeId (device ID)
            // and uses the entity's node ID for the `ObjectId`:
            //
            //      BaseTopic / NodeId / ObjectId / Component / TopicAction

            BaseTopic = elements[0];
            NodeId = elements[1];
            ObjectId = elements[2];
            TopicAction = elements[3].ToTopicAction();
            Component = "";
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    public MqttTopic BaseTopic { get; private set; } = "";

    public string Component { get; private set; } = "";

    public bool IsValidNet2HassMessage => BaseTopic.IsValid &&
                                          Component == "" &&
                                          NodeId.IsValid && ObjectId.IsValid &&
                                          TopicAction != TopicAction.None;

    /// <summary>
    ///     NodeId is the device ID.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Home Assistant discovery ignores this part of the discovery message topics.
    ///         It is used for state update and set command message topics to the Net2HassMqtt client.
    ///     </para>
    /// </remarks>
    public MqttTopic NodeId { get; private set; } = "";

    /// <summary>
    ///     In a Home Assistant discovery topic this is the entity's `unique_id` or `object_id`
    ///     In state update and set command message topics to the Net2HassMqtt client it is the entity's node ID.
    /// </summary>
    public MqttTopic ObjectId { get; private set; } = ""; // todo: HASS has depreciated use of object_id in favour of using default_entity_id. No longer used after 2026.4.

    public TopicAction TopicAction { get; private set; } = TopicAction.None;

    /// <summary>
    ///     Build a Home Assistant discovery topic.
    /// </summary>
    public MqttTopic BuildHassDiscoveryTopic()
    {
        if (!BaseTopic.IsValid)
        {
            throw new Net2HassMqttInvalidTopicException("Topic requires a base topic.");
        }

        if (string.IsNullOrWhiteSpace(Component))
        {
            throw new Net2HassMqttInvalidTopicException("Topic requires a component.");
        }

        if (!NodeId.IsValid)
        {
            throw new Net2HassMqttInvalidTopicException("Topic requires a node ID.");
        }

        if (!ObjectId.IsValid)
        {
            throw new Net2HassMqttInvalidTopicException("Topic requires an object ID.");
        }

        if (TopicAction == TopicAction.None)
        {
            throw new Net2HassMqttInvalidTopicException("Topic requires an action.");
        }

        return BaseTopic / Component / NodeId / ObjectId / TopicAction;
    }

    /// <summary>
    ///     Build a Net2HassMqtt state topic.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Net2HasMqtt forces the entity's node ID (and therefore `ObjectId`) to be unique within the device.
    ///         Not just within the entity's Home Assistant domain (e.g: `binary_sensor`). This allows a more readable grouping
    ///         by device ID (`NodeId`).
    ///     </para>
    ///     <para>
    ///         This is similar to the Zigbee2Mqtt's topic structure but different to Home Assistant's discovery topic
    ///         structure.
    ///     </para>
    /// </remarks>
    public MqttTopic BuildStateTopic()
    {
        return BaseTopic / NodeId / ObjectId;
    }

    /// <summary>
    ///     Build a Net2HassMqtt state topic.
    /// </summary>
    public MqttTopic BuildCommandTopic()
    {
        return BaseTopic / NodeId / ObjectId / TopicAction.SetCmd;
    }

    public TopicBuilder Clone()
    {
        return new TopicBuilder(this);
    }

    public static TopicBuilder Parse(string topic)
    {
        return new TopicBuilder(topic);
    }

    public TopicBuilder WithAction(TopicAction action)
    {
        if (TopicAction != TopicAction.None)
        {
            throw new Net2HassMqttInvalidTopicException($"TopicAction is already set to {TopicAction}. Cannot be changed to {action}.");
        }

        TopicAction = action;
        return this;
    }

    public TopicBuilder WithAnyComponent()
    {
        if (!string.IsNullOrWhiteSpace(Component))
        {
            throw new Net2HassMqttInvalidTopicException($"Topic Component is already set to {Component}. Cannot be changed to Any.");
        }

        Component = "+";
        return this;
    }

    public TopicBuilder WithAnyObjectId()
    {
        WithObjectId("+");
        return this;
    }

    public TopicBuilder WithBaseTopic(string baseTopic)
    {
        WithBaseTopic((MqttTopic)baseTopic);
        return this;
    }

    public TopicBuilder WithBaseTopic(MqttTopic baseTopic)
    {
        if (BaseTopic.IsValid)
        {
            throw new Net2HassMqttInvalidTopicException($"TopicAction is already set to {BaseTopic}. Cannot be changed to {baseTopic}.");
        }

        BaseTopic = baseTopic;
        return this;
    }

    public TopicBuilder WithComponent(string component)
    {
        if (!string.IsNullOrWhiteSpace(Component))
        {
            throw new Net2HassMqttInvalidTopicException($"Topic Component is already set to {Component}. Cannot be changed to {component}.");
        }

        Component = component;
        return this;
    }

    public TopicBuilder WithNodeId(string nodeId)
    {
        WithNodeId((MqttTopic)nodeId);
        return this;
    }

    public TopicBuilder WithNodeId(MqttTopic nodeId)
    {
        if (NodeId.IsValid)
        {
            throw new Net2HassMqttInvalidTopicException($"Topic NodeId is already set to {NodeId}. Cannot be changed to {nodeId}.");
        }

        NodeId = nodeId;
        return this;
    }

    public TopicBuilder WithObjectId(string objectId)  // todo: HASS has depreciated use of object_id in favour of using default_entity_id. No longer used after 2026.4.
    {
        if (ObjectId.IsValid)
        {
            throw new Net2HassMqttInvalidTopicException($"Topic NodeId is already set to {ObjectId}. Cannot be changed to {objectId}.");
        }

        ObjectId = objectId;
        return this;
    }
}