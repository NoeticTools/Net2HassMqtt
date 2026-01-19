using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

internal sealed class HassDomainsSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string OutputFilename = "Configuration/HassDomain.g.cs";

    private const string ContentTemplate =
        """
        using System.CodeDom.Compiler;
        using System.Dynamic;


        namespace NoeticTools.Net2HassMqtt.Configuration;
        
        /// <summary>
        ///   A Home Assistant domain.
        /// </summary>
        /// <remarks>
        ///   Obtain a HassDomain instance from the <see cref="HassDomains"/> class.
        /// </remarks>
        /// <seealso href="https://www.home-assistant.io/integrations/homeassistant/#device-class">Home Assistant Domains</seealso>
        [GeneratedCode("HassTypesSourceGenerator", "0.2.0")]
        public sealed class HassDomain
        {
            internal HassDomain(string hassDomainName, string domainName)
            {
                HassDomainName = hassDomainName;
                DomainName = domainName;
            }
            
            /// <summary>
            /// The Home Assistant entity domain (e.g: switch). This is only used for entities (not used by attributes) and is in lizard_case.
            /// </summary>
            public string HassDomainName { get; }
            
            /// <summary>
            /// The UpperCamel case version of the snake_case Home Assistant entity domain (e.g: switch). This is only used for entities (not used by attributes) and is in lizard_case.
            /// </summary>
            public string DomainName { get; }
        }
        
        /// <summary>
        ///   The Home Assistant <a href"https://www.home-assistant.io/integrations/homeassistant/#device-class">domains</a>.
        /// </summary>
        [GeneratedCode("HassTypesSourceGenerator", "0.2.0")]
        public sealed class HassDomains
        {
        {{~ for domainInfo in Domains ~}}
            public static HassDomain {{domainInfo.DomainName}} => new HassDomain("{{domainInfo.HassDomainName}}", "{{domainInfo.DomainName}}");

        {{ end ~}}
        
            public static HassDomain GetByHassDomainName(string domainName)
            {
                switch(domainName)
                {
        {{~ for domainInfo in Domains ~}}
                    case "{{domainInfo.DomainName}}":
                        return HassDomains.{{domainInfo.DomainName}};
                        break;
                    
        {{ end ~}}
                    default:
                        throw new KeyNotFoundException($"HassDomain with DomainName '{{domainName}}' not found.");
                }
            }
        }
        """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        context.RenderAndAddToSource(new
                                     {
                                         domainContext.Domains
                                     },
                                     OutputFilename,
                                     ContentTemplate!);
    }
}