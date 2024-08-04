using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;


namespace NoeticTools.Net2HassMqtt.Entities;

/// <summary>
///     An on/off switch.
/// </summary>
/// <remarks>
///     <para>Monitors a switch's on/off state and allows controlling the switch.</para>
///     <para>
///         <b>Entity status</b>
///     </para>
///     <para>The entity status is read from the (optional) provided model property.</para>
///     <para>Valid values by property type and configured unit of measurement are:</para>
///     <list type="bullet">
///         <item>
///             <term>bool</term>
///             <c>true</c> when value is open, <c>false</c> when valve is closed.
///         </item>
///         <item>
///             <term>string</term><c>'opened'</c> when value is open, <c>'closed'</c> when valve is closed.
///         </item>
///     </list>
///     <param>Any other property values may be converted to a string and will probably be ignored by Home Assistant.</param>
///     <para>
///         <b>Commands</b>
///     </para>
///     <para>The entity commands are passed to the (optional) provided model property.</para>
///     <para>Command values by property type are:</para>
///     <list type="bullet">
///         <item>
///             <term>bool</term>
///             <c>true</c> to open the valve, <c>false</c> to close the valve.
///         </item>
///         <item>
///             <term>string</term><c>'OPEN'</c> to open the valve, <c>'CLOSE'</c> to close the valve.
///         </item>
///     </list>
///     <param>Any other property values may be converted to a string and will probably be ignored by Home Assistant.</param>
///     <para>The required command method signatures is:</para>
///     <code>
///     [return type] &lt;MethodName&gt;(&lt;ValueType&gt; command);
/// </code>
///     <para>Any return value is ignored.</para>
///     <para>
///         <b>Model code examples</b>
///     </para>
///     <para>Example model code:</para>
///     <code>
///     public class MyValve : ObservableObject
///     {
///             :
///         public bool IsOpen
///         {
///             get{ /* observable property implementation */ }
///         }
/// 
///         public void Operate(string command)
///         {
///             // code here to handle open or close command.
///         }
///            :
///     }
/// </code>
///     Or:
///     <code>
///     void OpenClose(bool open);
/// </code>
///     <para>
///         <i>Home Assistant references:</i>
///     </para>
///     <list type="bullet">
///         <item>
///             <a href="https://www.home-assistant.io/integrations/switch">Home Assistant Switch</a>
///         </item>
///         <item>
///             <a href="https://www.home-assistant.io/integrations/switch.mqtt">Home Assistant Switch MQTT</a>
///         </item>
///         <item>
///             <a href="https://www.home-assistant.io/integrations/mqtt/#mqtt-discovery">Home Assistant MQTT discovery</a>
///         </item>
///     </list>
/// </remarks>
internal sealed class SwitchEntity : StateEntityBase<SwitchConfig>
{
    public SwitchEntity(SwitchConfig config, string entityUniqueId, string deviceNodeId, INet2HassMqttClient mqttClient, ILogger logger) :
        base(config, entityUniqueId, deviceNodeId, mqttClient, logger)
    {
    }

    protected override EntityConfigMqttJsonBase GetHasDiscoveryMqttPayload(DeviceConfig deviceConfig)
    {
        return new SwitchConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}