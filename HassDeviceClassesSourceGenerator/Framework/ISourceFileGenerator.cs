using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.Framework;

internal interface ISourceFileGenerator<T>
{
    void Generate(ImmutableArray<T> pages, FileSourceGeneratorContext context, DomainsContext domainContext);
}