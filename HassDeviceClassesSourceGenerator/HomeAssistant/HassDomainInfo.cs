using System.Collections.Generic;


namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal sealed class HassDomainInfo
{
    public HassDomainInfo(string hassDomain, bool isReadOnly, 
                          bool commandHandlerIsRequired, 
                          IReadOnlyList<HassDeviceClassInfo> deviceClasses,
                          bool hasRetainOption,
                          List<AdditionalOptionInfo> additionalOptions)
    {
        HassDomainName = hassDomain;
        IsReadOnly = isReadOnly;
        CommandHandlerIsRequired = commandHandlerIsRequired;
        DeviceClasses = deviceClasses;
        HasRetainOption = hasRetainOption;
        AdditionalOptions = additionalOptions;
        DomainName = UoMSnakeCaseTransformer.ToUpperCamelCase(hassDomain);
    }

    public bool CommandHandlerIsRequired { get; }

    public string DomainName { get; }

    public string HassDomainName { get; }

    public bool IsReadOnly { get; }

    public IReadOnlyList<HassDeviceClassInfo> DeviceClasses { get; }

    public bool HasRetainOption { get; }

    public IReadOnlyList<AdditionalOptionInfo> AdditionalOptions { get; }
}