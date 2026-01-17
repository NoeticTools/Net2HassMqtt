using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class EntityBuildersSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string ContentTemplate =
        """
        using System.ComponentModel;
        using NoeticTools.Net2HassMqtt.Configuration;
        using NoeticTools.Net2HassMqtt.Configuration.Building;
        using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
        #nullable enable


        namespace NoeticTools.Net2HassMqtt.Configuration.Building;

        {{~
            HassDomainName = domain.HassDomainName
            DomainName = domain.DomainName
            EntityConfigClass = ( DomainName | string.append "Config" )
            IsReadOnly = domain.IsReadOnly
        ~}}
        public abstract partial class {{DomainName}}EntityBuilder<T,TC> : EntityBuilderBase<T, TC>
            where T : EntityBuilderBase<T, TC>
            where TC : {{EntityConfigClass}}
        {
            internal {{DomainName}}EntityBuilder(TC entityConfig)
                : base(entityConfig)
            {
            }
        
            {{~ if IsReadOnly == false ~}}
            /// <summary>
            ///     The name of the command handler method on the model that handles entity commands like "open/close".
            ///     Use <c>nameof(&lt;model&gt;.&lt;statusProperty&gt;)</c>.
            /// </summary>
            public T WithCommandMethod(string methodName)
            {
                EntityConfig.CommandMethodName = methodName;
                return (this as T)!;
            }
            {{~ end ~}}
            {{~ for option in domain.AdditionalOptions ~}}
            
            /// <summary>
            ///    {{option.Description}} ({{ option.IsOptional ? "Optional" : "Required" }}, default is '{{option.DefaultValue}}')
            /// </summary>
            public T With{{option.Name}}({{option.ValueType}} value)
            {
                EntityConfig.{{option.Name}} = value;
                return (this as T)!;
            }
            {{~ end ~}}
        }

        {{~ for deviceClass in domain.DeviceClasses ~}}
        {{~
            DeviceClass = deviceClass.DeviceClass
            UoMClass = deviceClass.UoMClassName
            EntityBuilderClass = ( DeviceClass | string.append DomainName | string.append "EntityBuilder" )
            EntityConfigClass = ( DeviceClass | string.append DomainName | string.append "Config" )
        }}
        public sealed partial class {{EntityBuilderClass}} : {{DomainName}}EntityBuilder<{{EntityBuilderClass}}, {{EntityConfigClass}}>
        {
            internal {{EntityBuilderClass}}({{EntityConfigClass}} entityConfig)
                : base(entityConfig)
            {
                {{~ if DeviceClass == "None" || deviceClass.HassUnitsOfMeasurement.size == 0 ~}}
                EntityConfig.UnitOfMeasurement = new {{UoMClass}}("None");
                {{~ else if deviceClass.HassUnitsOfMeasurement.size == 1 ~}}
                EntityConfig.UnitOfMeasurement = new {{UoMClass}}(HassUoMs.{{deviceClass.HassUnitsOfMeasurement[0].PropertyName}});
                {{~ end ~}}
            }
        
            public {{EntityBuilderClass}} WithUnitOfMeasurement({{UoMClass}} unitOfMeasurement)
            {
                EntityConfig.UnitOfMeasurement = unitOfMeasurement;
                return this;
            }
        }
        {{~ end ~}}
        """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        foreach (var domain in domainContext.Domains)
        {
            var generatedClassName = $"{domain.DomainName}EntityBuilder";
            context.RenderAndAddToSource(new
                                         {
                                             domain
                                         },
                                         $"Configuration/Builders/{generatedClassName}.g.cs",
                                         ContentTemplate!);
        }
    }
}