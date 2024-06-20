using System.Collections.Generic;
using System.Linq;


namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal sealed class HassDeviceClassInfo
{
    public HassDeviceClassInfo(string hassDomain, string hassDeviceClass, string description,
                               bool isReadOnly,
                               IReadOnlyList<string> hassUnitsOfMeasurement)
    {
        HassDomainName = hassDomain.ToMqttTopicSnakeCase();
        DomainName = hassDomain.ToUpperCamelCase();
        HassDeviceClassName = hassDeviceClass;
        DeviceClass = hassDeviceClass.ToUpperCamelCase();
        Description = description;
        IsReadOnly = isReadOnly;
        HassUnitsOfMeasurement = hassUnitsOfMeasurement.Select(x => new HassUnitOfMeasurementInfo(x)).ToList();
        UoMClassName = GetUoMClassName();
    }

    public string UoMClassName { get; }

    public string Description { get; }

    public bool IsReadOnly { get; }

    public string DeviceClass { get; }

    public string DomainName { get; }

    public string? HassDeviceClassName { get; }

    public string HassDomainName { get; }

    public List<HassUnitOfMeasurementInfo> HassUnitsOfMeasurement { get; }

    private string GetUoMClassName()
    {
        if (DeviceClass == DomainName)
        {
            return $"{DomainName}UoM";
        }

        if (DeviceClass == "None")
        {
            return $"Default{DomainName}UoM";
        }

        return $"{DeviceClass}{DomainName}UoM";
    }
}