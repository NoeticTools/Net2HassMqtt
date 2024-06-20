using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;


namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal sealed class DomainsContext
{
    public DomainsContext(ImmutableArray<DeviceClassModel> deviceClassModels, SourceProductionContext context)
    {
        var domains = new List<HassDomainInfo>();
        var byDomainDeviceClasses = new List<HassDeviceClassInfo>();
        foreach (var deviceClassModel in deviceClassModels)
        {
            var info = DeviceClassInfoParser.Parse(deviceClassModel.DeclarationText);
            domains.AddRange(info.domains.Values);
            byDomainDeviceClasses.AddRange(info.allDeviceClasses);
        }

        ValidateUniqueDomainNames(context, domains);
        DeviceClasses = byDomainDeviceClasses;
        Domains = domains;
    }

    public IReadOnlyList<HassDeviceClassInfo> DeviceClasses { get; }

    public IReadOnlyList<HassDomainInfo> Domains { get; }

    private static void ValidateUniqueDomainNames(SourceProductionContext context, List<HassDomainInfo> domains)
    {
        if (domains.Count != domains.Select(x => x.DomainName).Distinct().Count())
        {
            const string message = "Found duplicate domains.";
            Console.Error.WriteLine(message);
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ConfigurationError, null, message));
            domains.Clear();
        }
    }
}