using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class DeviceBuilderSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string ContentTemplate =
        """
        using System.CodeDom.Compiler;
        using System.ComponentModel;
        using System.Diagnostics.CodeAnalysis;
        using NoeticTools.Net2HassMqtt.Configuration.Building;
        using NoeticTools.Net2HassMqtt.Configuration.UnitsOfMeasurement;
        // ReSharper disable CheckNamespace
        #nullable enable


        namespace NoeticTools.Net2HassMqtt.Configuration.Building;

        [GeneratedCode("HassTypesSourceGenerator", "0.1.0")]
        public partial class DeviceBuilder
        {
        {{~ for domain in Domains ~}}
        {{~ DomainName = domain.DomainName ~}}
        {{~ for deviceClass in domain.DeviceClasses ~}}
        {{~
            DeviceClass = deviceClass.DeviceClass
            UoMClass = deviceClass.UoMClassName
            DomainDeviceClass = ( DomainName | string.append "DeviceClass." | string.append DeviceClass )
            DomainEntityConfigClass = ( DomainName | string.append "Config" )
            EntityBuilderClass = ( DeviceClass | string.append DomainName | string.append "EntityBuilder" )
        
            if domain.IsReadOnly == true
                readOnlyEntityText = $" {deviceClass.HassDomainName} is a read only domain. It does not support command methods."
                andOrCommandMethodText = ""
            else
                readOnlyEntityText = ""
                andOrCommandMethodText = "and/or command method"
            end
        
            if DeviceClass == "None" || DeviceClass == DomainName
                methodName = ( "Has" | string.append DomainName )
                
                    methodDocumentationXml = $"    /// <summary>
            ///     Add a Home Assistant <a href=\"https://www.home-assistant.io/integrations/{deviceClass.HassDomainName}.mqtt/\">{deviceClass.HassDomainName}</a> domain entity
            ///     using the domain's default unit of measurement.
            /// </summary>
            /// <remarks>
            ///     Maps the status property{andOrCommandMethodText} to a Home Assistant {deviceClass.HassDomainName} domain entity that uses the domain's default unit of measurement.
            ///     {readOnlyEntityText}
            /// </remarks>"
            
            else
                methodName = ( ("Has" | string.append DeviceClass) | string.append DomainName )
                
                methodDocumentationXml = $"    /// <summary>
            ///     Add a Home Assistant <a href=\"https://www.home-assistant.io/integrations/{deviceClass.HassDomainName}.mqtt/\">{deviceClass.HassDomainName}</a> domain entity
            ///     using a {UoMClass} unit of measurement.
            /// </summary>
            /// <remarks>
            ///     Maps the status property{andOrCommandMethodText} to a Home Assistant {deviceClass.HassDomainName} domain entity, with
            ///     a {UoMClass} unit of measurement.
            ///
            ///     The <a href=\"https://www.home-assistant.io/integrations/homeassistant/#device-class\">device class</a> provides classification of what the device is.
            ///     Device classes can come with unit of measurement, default icon, and supported features.
            /// </remarks>"
            end
        ~}}
        {{methodDocumentationXml}}
            public DeviceBuilder {{methodName}}(Func<{{EntityBuilderClass}}, {{EntityBuilderClass}}> configureFunc)
            {
                var entityConfig = new {{DomainEntityConfigClass}}({{DomainDeviceClass}});
                var entityBuilder = new {{EntityBuilderClass}}(entityConfig);
                entityConfig = configureFunc(entityBuilder).EntityConfig;
                entityConfig.Validate();
                DeviceConfig.AddEntity(entityConfig.EntityNodeId, entityConfig);
                return this;
            }

        {{~ end ~}}
        {{~ end ~}}
        }
        """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        context.RenderAndAddToSource(new
                                     {
                                         domainContext.Domains
                                     },
                                     "Configuration/Builders/DeviceBuilder.g.cs",
                                     ContentTemplate);
    }
}