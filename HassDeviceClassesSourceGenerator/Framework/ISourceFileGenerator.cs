using System.Collections.Immutable;
using HassTypesSourceGenerator.HomeAssistant;


namespace HassTypesSourceGenerator.Framework;

internal interface ISourceFileGenerator<T>
{
    void Generate(ImmutableArray<T> pages, FileSourceGeneratorContext context, DomainsContext domainContext);
}