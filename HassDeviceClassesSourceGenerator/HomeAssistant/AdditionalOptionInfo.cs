namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal class AdditionalOptionInfo
{
    public string Name { get; }
    public string MqttName { get; }
    public string Description { get; }
    public string ValueType { get; }
    public string DefaultValue { get; }
    public bool IsOptional { get; }

    public AdditionalOptionInfo(string name, string mqttName, string description, string valueType, string defaultValue, bool isOptional)
    {
        Name = name;
        MqttName = mqttName;
        Description = description;
        ValueType = valueType;
        DefaultValue = defaultValue;
        IsOptional = isOptional;
    }
}