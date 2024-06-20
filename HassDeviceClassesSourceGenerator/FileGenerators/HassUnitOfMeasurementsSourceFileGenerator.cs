using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

internal sealed class HassUnitOfMeasurementsSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string Filename = "HassUoMs.g.cs";

    private const string ContentTemplate = """
                                           using System.CodeDom.Compiler;
                                           // ReSharper disable CheckNamespace


                                           namespace NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;

                                           /// <summary>
                                           ///     Home Assitant units of measurement.
                                           /// </summary>
                                           [GeneratedCode("HassTypesSourceGenerator", "0.1.0")]
                                           public partial class HassUoMs
                                           {
                                           {{~ for unitOfMeasurement in HassUnitsOfMeasurement ~}}
                                               public const string {{unitOfMeasurement.PropertyName}} = "{{unitOfMeasurement.HassUoM}}";
                                           {{~ end ~}}

                                           {{TransformMethod}}
                                           }
                                           """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels,
                         FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        var unitLookup = new Dictionary<string, UoMDesc>();
        foreach (var deviceClass in domainContext.DeviceClasses)
        {
            foreach (var unitOfMeasurement in deviceClass.HassUnitsOfMeasurement
                                                         .Where(unitOfMeasurement => !unitLookup.ContainsKey(unitOfMeasurement.PropertyName)))
            {
                unitLookup.Add(unitOfMeasurement.PropertyName,
                               new UoMDesc(unitOfMeasurement.PropertyName, unitOfMeasurement.HassUoM));
            }
        }

        var hassUnitOfMeasurements =
            unitLookup.Values.Where(x => !string.IsNullOrWhiteSpace(x.HassUoM)).OrderBy(x => x.PropertyName).Distinct().ToList();
        context.RenderAndAddToSource(new
                                     {
                                         HassUnitsOfMeasurement = hassUnitOfMeasurements
                                     },
                                     Filename,
                                     ContentTemplate);
    }

    public class UoMDesc
    {
        public UoMDesc(string propertyName, string hassUoM)
        {
            PropertyName = propertyName;
            HassUoM = hassUoM;
        }

        public string HassUoM { get; }
        public string PropertyName { get; }
    }
}