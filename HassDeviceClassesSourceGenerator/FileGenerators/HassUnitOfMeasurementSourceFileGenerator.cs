using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class HassUnitOfMeasurementSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string ContentTemplate = """
                                           using System.CodeDom.Compiler;
                                           // ReSharper disable CheckNamespace


                                           namespace NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;

                                           /// <summary>
                                           ///     Home Assitant unit of measurement for domain "{{HassDomainName}}"'s <a href="https://www.home-assistant.io/integrations/sensor#device-class">device_class</a> "{{HassDeviceClassName}}".
                                           ///     {{ Description ~}}
                                           /// </summary>
                                           [GeneratedCode("HassTypesSourceGenerator", "0.1.0")]
                                           public partial class {{UoMClassName}} : UnitOfMeasurement
                                           {
                                               internal {{UoMClassName}}(string hassUnitOfMeasurement) : base(hassUnitOfMeasurement)
                                               {
                                               }
                                               
                                           {{~ if HassUnitsOfMeasurement.size == false ~}}
                                               /// <summary>
                                               /// Home Assistant domain "{{HassDomainName}}"'s inferred default unit of measurement.
                                               /// </summary>
                                               public static {{UoMClassName}} None { get; } = new("none");
                                           {{~ end ~}}
                                           {{~ for unitOfMeasurement in HassUnitsOfMeasurement ~}}
                                           {{~ if unitOfMeasurement.HassUoM | string.empty == false ~}}
                                               /// <summary>
                                               /// Home Assistant domain "{{HassDomainName}}"'s unit of measurement "{{unitOfMeasurement.HassUoM}}".
                                               /// </summary>
                                               public static {{UoMClassName}} {{unitOfMeasurement.PropertyName}} { get; } = new(HassUoMs.{{unitOfMeasurement.PropertyName}});
                                               
                                           {{~ end ~}}
                                           {{~ end ~}}
                                           }
                                           """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        foreach (var deviceClass in domainContext.DeviceClasses)
        {
            var filename = $"Configuration/UnitsOfMeasurement/{deviceClass.UoMClassName}.g.cs";

            context.RenderAndAddToSource(new
                                         {
                                             deviceClass.HassDeviceClassName,
                                             deviceClass.HassDomainName,
                                             deviceClass.UoMClassName,
                                             deviceClass.Description,
                                             deviceClass.HassUnitsOfMeasurement
                                         },
                                         filename,
                                         ContentTemplate);
        }
    }
}