namespace NoeticTools.Net2HassMqtt.Configuration.Building;

public sealed partial class DeviceBuilder : IEntityUniqueIdBuilder
{
    public DeviceBuilder()
    {
        DeviceConfig = new DeviceConfig();
    }

    internal DeviceBuilder(DeviceConfig deviceConfig)
    {
        DeviceConfig = deviceConfig;
    }

    internal DeviceConfig DeviceConfig { get; }

    string IEntityUniqueIdBuilder.BuildEntityUniqueId(string entityNodeId)
    {
        return DeviceConfig.BuildEntityUniqueId(entityNodeId);
    }

    /// <summary>
    ///     Set required device name.
    ///     This is the friendly name that will be seen in Home Assistant.
    /// </summary>
    public DeviceBuilder WithFriendlyName(string name)
    {
        DeviceConfig.Name = name;
        return this;
    }

    /// <summary>
    ///     Set required device ID.
    ///     This will be the prefix for all device entity IDs.
    ///     It must be unique and never change.
    /// </summary>
    public DeviceBuilder WithId(string nodeId)
    {
        DeviceConfig.Identifiers = new[] { nodeId };
        return this;
    }
}