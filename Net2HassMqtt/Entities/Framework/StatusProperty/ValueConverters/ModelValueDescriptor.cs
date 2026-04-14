namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty.ValueConverters;

internal sealed class ModelValueDescriptor(Type sourceType, string hassDomainName, string? hassDeviceClass, string? hassUoM, 
                                    string modelPropertyName, Type modelPropertyType)
{
    public Type SourceType { get; } = sourceType;

    public string HassDomainName { get; } = hassDomainName;

    public string? HassDeviceClass { get; } = hassDeviceClass;

    public string? HassUoM { get; } = hassUoM;

    public string ModelPropertyName { get; } = modelPropertyName;

    public Type ModelPropertyType { get; } = modelPropertyType;
}