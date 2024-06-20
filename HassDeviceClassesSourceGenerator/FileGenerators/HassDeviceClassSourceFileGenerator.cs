using System.Collections.Generic;
using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;
using Microsoft.CodeAnalysis.CSharp;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class HassDeviceClassSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string ContentTemplate = """
                                           using System.CodeDom.Compiler;
                                           // ReSharper disable CheckNamespace


                                           namespace NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;

                                           /// <summary>
                                           ///     The Home Assistant (HASS) "{{HassDomainName}}" plaform device class.
                                           /// </summary>
                                           /// <seealso cref="https://www.home-assistant.io/integrations/homeassistant/#device-class" />
                                           [GeneratedCode("HassTypesSourceGenerator", "0.1.0")]
                                           public sealed class {{ClassName}}
                                           {
                                               public string HassDeviceClassName { get; }
                                           
                                               private {{ClassName}}(string deviceClass)
                                               {
                                                   HassDeviceClassName = deviceClass;
                                               }
                                               {{DeclarationsCodeFragment}}
                                               
                                           }
                                           """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        foreach (var domain in domainContext.Domains)
        {
            var domainDeviceClassName = $"{domain.DomainName}DeviceClass";
            var descriptorDeclarationCodeLines = new List<string>();
            var deviceClasses = domain.DeviceClasses;
            var hassDomain = domain.HassDomainName;

            foreach (var deviceClassInfo in deviceClasses)
            {
                var propertyName = UoMSnakeCaseTransformer.ToUpperCamelCase($"{deviceClassInfo.HassDeviceClassName}"); // todo - use scriban
                if (deviceClassInfo.HassDeviceClassName == "None")
                {
                    descriptorDeclarationCodeLines.Add($$$"""
                                                          
                                                              /// <summary>
                                                              /// Home Assistant domain "{{{deviceClassInfo.HassDomainName}}}"'s device class "{{{deviceClassInfo.HassDeviceClassName}}}".
                                                              /// </summary>
                                                              public static {{{domainDeviceClassName}}} {{{propertyName}}} { get; } = new(null);
                                                          """);
                }
                else
                {
                    descriptorDeclarationCodeLines.Add($$$"""
                                                          
                                                              /// <summary>
                                                              /// Home Assistant domain "{{{deviceClassInfo.HassDomainName}}}"'s device class "{{{deviceClassInfo.HassDeviceClassName}}}".
                                                              /// </summary>
                                                              public static {{{domainDeviceClassName}}} {{{propertyName}}} { get; } = new("{{{deviceClassInfo.HassDeviceClassName}}}");
                                                          """);
                }
            }

            var declarationsCodeFragment = string.Join("\n", descriptorDeclarationCodeLines);
            var content = ContentTemplate.Replace("{{HassDomainName}}", hassDomain)
                                         .Replace("{{DeclarationsCodeFragment}}", declarationsCodeFragment)
                                         .Replace("{{ClassName}}", domainDeviceClassName);

            content = CSharpSyntaxTree.ParseText(content).GetRoot().ToFullString();

            var filename = $"DeviceClasses/{domainDeviceClassName}.g.cs";
            context.AddSource(filename, content);
        }
    }
}