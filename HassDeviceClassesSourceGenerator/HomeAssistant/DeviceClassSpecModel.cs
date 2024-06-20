namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal struct DeviceClassModel
{
    public string Namespace { get; }
    public string HassClassName { get; }
    public string DeclarationText { get; }

    public DeviceClassModel(string @namespace, string hassClassName, string declarationText)
    {
        Namespace = @namespace;
        HassClassName = hassClassName;
        DeclarationText = declarationText;
    }
}