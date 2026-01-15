using System.Collections.Immutable;
using HomeAssistantTypesSourceGenerator.Framework;
using HomeAssistantTypesSourceGenerator.HomeAssistant;


namespace HomeAssistantTypesSourceGenerator.FileGenerators;

internal sealed class EntityDomainConfigMqttSourceFileGenerator : ISourceFileGenerator<DeviceClassModel>
{
    private const string ContentTemplate =
        """
        using System.ComponentModel;
        using System.Text.Json.Serialization;
        using NoeticTools.Net2HassMqtt.Configuration;
        using NoeticTools.Net2HassMqtt.Mqtt.Topics;
        #nullable enable

        namespace NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;

        {{~
            HassDomainName = domain.HassDomainName
            DomainName = domain.DomainName
            IsReadOnly = domain.IsReadOnly
            CommandHandlerIsRequired = domain.CommandHandlerIsRequired
            AdditionalOptions = domain.AdditionalOptions
            HasRetainOption = domain.HasRetainOption
        ~}}
        /// <summary>
        ///     Home Assistant {{DomainName}} entity discovery configuration.
        /// </summary>
        /// <remarks>
        ///     <para>Used for Home Assistant entity discovery and to update the entity's configuration.</para>
        ///     <para>See also:</para>
        ///     <list type="bullet">
        ///         <item>
        ///             <a href="https://www.home-assistant.io/integrations/{{HassDomainName}}.mqtt">Home Assistant Cover MQTT</a>
        ///         </item>
        ///         <item>
        ///             <a href="https://home-assistant-china.github.io/docs/mqtt/discovery/">Home Assistant MQTT discovery</a>
        ///         </item>
        ///     </list>
        /// </remarks>
        internal class {{DomainName}}ConfigMqttJson : EntityConfigMqttJsonBase
        {
            public {{DomainName}}ConfigMqttJson(string entityUniqueId, {{DomainName}}Config config, DeviceConfig deviceConfig, string mqttClientId) :
                base(config, entityUniqueId, deviceConfig, mqttClientId)
            {
                DeviceClass = config.HassDeviceClassName!;
                UnitOfMeasurement = config.UnitOfMeasurement!.HassUnitOfMeasurement;
                {{~ if IsReadOnly == false ~}}
                CommandTopic = config.CommandTopic;
                {{~ end ~}}
                {{~ for option in AdditionalOptions ~}}
                {{option.Name}} = config.{{option.Name}};
                {{~ end ~}}
            }
            {{~ if IsReadOnly == false ~}}
            
            [JsonPropertyName("command_topic")]
            public string{{ CommandHandlerIsRequired ? "" : "?" }} CommandTopic { get; set; }
            {{~ end ~}}
        
            /// <summary>
            ///     Home Assistant sensor entity <a href="https://www.home-assistant.io/integrations/{{HassDomainName}}/#device-class">device class</a>.
            /// </summary>
            [JsonPropertyName("device_class")]
            public string DeviceClass { get; set; }
            {{~ for option in AdditionalOptions ~}}
            
            /// <summary>
            ///    {{option.Description}} ({{ option.IsOptional ? "Optional" : "Required" }}, default is '{{option.DefaultValue}}')
            /// </summary>
            [JsonPropertyName("{{option.MqttName}}")]
            {{~ if option.IsOptional ~}}
            public {{option.ValueType}}? {{option.Name}} { get; set; }
            {{~ else ~}}
            public {{option.ValueType}} {{option.Name}} { get; set; } = {{ option.DefaultValue }};
            {{~ end ~}}
            {{~ end ~}}
            
            /// <summary>
            ///     Unit of measure dependent on the <a href="https://www.home-assistant.io/integrations/{{HassDomainName}}/#device-class">device class</a>.
            /// </summary>
            [JsonPropertyName("unit_of_measurement")]
            public string? UnitOfMeasurement { get; set; }
            
            {{~ if HasRetainOption == true ~}}
            [JsonPropertyName("retain")]
            public bool Retain { get; set; } = true;
            
            {{~ end ~}}
            internal override void Build(TopicBuilder topic)
            {
                if (string.IsNullOrWhiteSpace(StateTopic))
                {
                    StateTopic = topic.BuildStateTopic().ToString();
                }
            
                if (string.IsNullOrWhiteSpace(JsonAttributesTopic))
                {
                    JsonAttributesTopic = StateTopic;
                }
                {{~ if IsReadOnly == false ~}}
                
                if (string.IsNullOrWhiteSpace(CommandTopic))
                {
                    CommandTopic = topic.BuildCommandTopic().ToString();
                }
                {{- end ~}}
            }
        }
        """;

    public void Generate(ImmutableArray<DeviceClassModel> deviceClassModels, FileSourceGeneratorContext context,
                         DomainsContext domainContext)
    {
        foreach (var domain in domainContext.Domains)
        {
            var generatedClassName = $"{domain.DomainName}ConfigMqtt";
            context.RenderAndAddToSource(new
                                         {
                                             domain
                                         },
                                         $"Mqtt/{generatedClassName}Json.g.cs",
                                         ContentTemplate!);
        }
    }
}