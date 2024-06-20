namespace NoeticTools.Net2HassMqtt.Configuration.Building;

public interface IEntityUniqueIdBuilder
{
    /// <summary>
    ///     <para>
    ///         Builds an entity unique ID (unique_id).
    ///     </para>
    ///     <para>
    ///         The ID uses the device's ID as a prefix and the node ID as the suffix as shown below:
    ///     </para>
    ///     <code>
    ///     &lt;Device ID&gt;&lt;Entity node ID&gt;
    ///     </code>
    ///     <para>
    ///         This ID must uniquely identify the entity within the MQTT broker and Home Assistant.
    ///         This value must not change between application shutdown and restart.
    ///     </para>
    /// </summary>
    string BuildEntityUniqueId(string entityNodeId);
}