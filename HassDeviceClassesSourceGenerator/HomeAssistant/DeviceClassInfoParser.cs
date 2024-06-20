using System;
using System.Collections.Generic;
using System.Linq;
using LightJson;
using LightJson.Serialization;


namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal static class DeviceClassInfoParser
{
    public static (IReadOnlyList<HassDeviceClassInfo> allDeviceClasses, Dictionary<string, HassDomainInfo> domains) Parse(string inputTextLines)
    {
        var domains = new Dictionary<string, HassDomainInfo>();
        var allDeviceClasses = new List<HassDeviceClassInfo>();

        var json = JsonReader.Parse(inputTextLines);
        foreach (var domainJson in json["domains"].AsJsonArray)
        {
            var jsonDomain = domainJson.AsJsonObject;
            var hassDomainName = jsonDomain["name"].AsString;
            var isReadOnly = jsonDomain["read_only"].AsBoolean;
            var requiresCommandHandler = jsonDomain.ContainsKey("requires_command_handler") && jsonDomain["requires_command_handler"].AsBoolean;
            var domainDeviceClasses = GetHassDeviceClassInfos(jsonDomain, hassDomainName, isReadOnly, allDeviceClasses);
            var additionalOptions = GetAdditionalOptions(jsonDomain);

            var domainInfo = new HassDomainInfo(hassDomainName, isReadOnly, requiresCommandHandler, domainDeviceClasses, additionalOptions);
            domains.Add(hassDomainName, domainInfo);
        }

        return (allDeviceClasses, domains);
    }

    private static List<AdditionalOptionInfo> GetAdditionalOptions(JsonObject jsonDomain)
    {
        var jsonAdditionalOptions = jsonDomain["additional_options"].AsJsonArray;

        var domainOptions = new List<AdditionalOptionInfo>();
        foreach (var jsonDomainOption in jsonAdditionalOptions)
        {
            var optionName = jsonDomainOption["name"].AsString;
            var mqttName = jsonDomainOption["mqtt_name"].AsString;
            var description = jsonDomainOption["description"].AsString;
            var valueType = jsonDomainOption["type"].AsString;
            var defaultValue = jsonDomainOption["default"].AsString;
            var isOptional = jsonDomainOption["is_optional"].AsBoolean;

            var optionInfo = new AdditionalOptionInfo(optionName, mqttName, description, valueType, defaultValue, isOptional);

            domainOptions.Add(optionInfo);
        }

        return domainOptions;
    }

    private static List<HassDeviceClassInfo> GetHassDeviceClassInfos(JsonObject jsonDomain, string hassDomainName, bool isReadOnly,
                                                                     List<HassDeviceClassInfo> allDeviceClasses)
    {
        var jsonDomainClasses = jsonDomain["domain_classes"].AsJsonArray;

        var domainDeviceClasses = new List<HassDeviceClassInfo>();
        foreach (var jsonDomainClass in jsonDomainClasses)
        {
            var hassDeviceClass = jsonDomainClass["name"].AsString;
            var description = jsonDomainClass["description"].AsString;
            var hassUnits = jsonDomainClass["units"].AsJsonArray.Select(x => x.AsString).Select(x => x.Trim().TrimEnd('.')).ToList();

            var hassDeviceClassInfo = new HassDeviceClassInfo(hassDomainName, hassDeviceClass, description, isReadOnly, hassUnits);
            allDeviceClasses.Add(hassDeviceClassInfo);

            domainDeviceClasses.Add(hassDeviceClassInfo);
        }

        return domainDeviceClasses;
    }
}