using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using HomeAssistantTypesSourceGenerator.FileGenerators;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;


#pragma warning disable RS1036

namespace HomeAssistantTypesSourceGenerator;

[Generator]
internal sealed class HassTypesSourceGenerator : SourceGeneratorBase, IIncrementalGenerator
{
    private const string AttributeName = "HassEntityDomainGeneratorAttribute";
    private const string AttributeFullyQualifiedName = "HomeAssistantTypesSourceGenerator.HassEntityDomainGeneratorAttribute";

    private static readonly IApiSourceFile[] Sources =
    [
        new PublishResourceFile("HassEntityDomainGeneratorAttribute.cs")
    ];

    private static readonly ISourceFileGenerator<DeviceClassModel>[] SourceFileGenerators =
    [
        new EntityDomainConfigMqttSourceFileGenerator(),
        new HassUnitOfMeasurementsSourceFileGenerator(),
        new EntityBuildersSourceFileGenerator(),
        new EntityDomainConfigSourceFileGenerator(),
        new DeviceBuilderSourceFileGenerator(),
        new HassDomainsSourceFileGenerator(),
        new HassUnitOfMeasurementSourceFileGenerator(),
        new HassDeviceClassSourceFileGenerator()
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static postInitializationContext =>
        {
            foreach (var sourceFile in Sources)
            {
                postInitializationContext.AddSource(sourceFile.Filename, SourceText.From(sourceFile.Content, Encoding.UTF8));
            }
        });

        var hassDomainsSpecPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(AttributeFullyQualifiedName,
                                                                                          static (_, _) => true,
                                                                                          static (context, _) => ToDeviceClassesModel(context));
        context.RegisterSourceOutput(hassDomainsSpecPipeline.Collect(), GenerateHassDomainsSourceFiles);
    }

    private static void GenerateHassDomainsSourceFiles(SourceProductionContext context, ImmutableArray<DeviceClassModel> domainsModels)
    {
        try
        {
            if (domainsModels.Length != 1)
            {
                var message = $"There may only be one '{AttributeFullyQualifiedName}' attribute per project. Found {domainsModels.Length}";
                Console.Error.WriteLine(message);
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ConfigurationError, null, message));
                return;
            }

            var domainsContext = new DomainsContext(domainsModels, context);
            var generatorContext = new FileSourceGeneratorContext(context);

            foreach (var generator in SourceFileGenerators)
            {
                generator.Generate(domainsModels, generatorContext, domainsContext);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Exception thrown: {exception.Message}");
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ExceptionThrownError, null, exception.Message));
        }
    }

    private static DeviceClassModel ToDeviceClassesModel(GeneratorAttributeSyntaxContext context)
    {
        var attribute = context.Attributes.First(x => x.AttributeClass!
                                                       .Name.EnsureEndsWith("Attribute")
                                                       .Equals(AttributeName));
        var classNode = (ClassDeclarationSyntax)context.TargetNode;
        var @namespace = GetNamespaceText(context.TargetSymbol.ContainingNamespace);
        var className = classNode.Identifier.Text;
        var attributeArguments = attribute.ConstructorArguments;
        var declarationText = (string)attributeArguments[0].Value!;
        return new DeviceClassModel(@namespace, className, declarationText);
    }
}