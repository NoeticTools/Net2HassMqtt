using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class EntityDomainConfigSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string ContentTemplate =
        """
        using System.ComponentModel;
        using NoeticTools.Net2HassMqtt.Configuration;
        using NoeticTools.Net2HassMqtt.Configuration.Building;
        using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
        #nullable enable


        namespace NoeticTools.Net2HassMqtt.Configuration;

        {{~
            HassDomainName = domain.HassDomainName
            DomainName = domain.DomainName
            IsReadOnly = domain.IsReadOnly
            CommandHandlerIsRequired = domain.CommandHandlerIsRequired
        ~}}
        public partial class {{DomainName}}Config : EntityConfigBase
        {
            internal {{DomainName}}Config({{DomainName}}DeviceClass deviceClass)
                : base(HassDomains.{{DomainName}}, deviceClass.HassDeviceClassName)
            {
                CommandHandlerIsRequired = {{CommandHandlerIsRequired}};
            }
        
            public string CommandTopic { get; set; } = "";
        
            {{~ if IsReadOnly == false ~}}
            /// <summary>
            ///     The name of the command handler method on the model that handles entity commands like "open/close".
            ///     Use <c>nameof(&lt;model&gt;.&lt;statusProperty&gt;)</c>.
            /// </summary>
            public {{DomainName}}Config WithCommandMethod(string methodName)
            {
                CommandMethodName = methodName;
                return this;
            }
            {{~ end ~}}
            {{~ for option in domain.AdditionalOptions ~}}
            
            /// <summary>
            ///    {{option.Description}} ({{ option.IsOptional ? "Optional" : "Required" }}, default is '{{option.DefaultValue}}')
            /// </summary>
            public {{option.ValueType}}{{ option.IsOptional ? "?" : "" }} {{option.Name}} { get; set; }
            {{~ end ~}}
        }

        {{~ for deviceClass in domain.DeviceClasses ~}}
        {{~
            DeviceClass = deviceClass.DeviceClass
            UoMClass = deviceClass.UoMClassName
            DomainDeviceConfigClass = ( DeviceClass | string.append DomainName | string.append "Config" )
        }}
        public sealed partial class {{DomainDeviceConfigClass}} : EntityConfigBase
        {
            internal {{DomainDeviceConfigClass}}({{DomainName}}DeviceClass deviceClass)
                : base(HassDomains.{{DomainName}}, deviceClass.HassDeviceClassName)
            {
            }
        }
        {{~ end ~}}
        """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        foreach (var domain in domainContext.Domains)
        {
            var generatedClassName = $"{domain.DomainName}Config";
            context.RenderAndAddToSource(new
                                         {
                                             domain
                                         },
                                         $"Configuration/{generatedClassName}.g.cs",
                                         ContentTemplate!);
        }
    }
}