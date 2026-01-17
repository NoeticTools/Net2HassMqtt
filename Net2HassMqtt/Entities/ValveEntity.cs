using Microsoft.Extensions.Logging;
using NoeticTools.Net2HassMqtt.Configuration;
using NoeticTools.Net2HassMqtt.Entities.Framework;
using NoeticTools.Net2HassMqtt.Framework;
using NoeticTools.Net2HassMqtt.Mqtt;
using NoeticTools.Net2HassMqtt.Mqtt.Payloads.Discovery;
using NoeticTools.Net2HassMqtt.Mqtt.Topics;


namespace NoeticTools.Net2HassMqtt.Entities;

/// <summary>
///     An open/close valve.
/// </summary>
/// <remarks>
///     <para>
///         A valve that is either open or closed. Position control, and opening and closing reporting options are not
///         supported.
///     </para>
///     <para>
///         <b>Entity status</b>
///     </para>
///     <para>The entity status is read from the (optional) provided model property. Valid values by property type are:</para>
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
///             <a href="https://www.home-assistant.io/integrations/valve.mqtt/">Home Assistant Valve MQTT</a>
///         </item>
///         <item>
///             <a href="https://home-assistant-china.github.io/docs/mqtt/discovery/">Home Assistant MQTT discovery</a>
///         </item>
///     </list>
/// </remarks>
internal sealed class ValveEntity(
    ValveConfig config,
    string entityUniqueId,
    string deviceNodeId,
    INet2HassMqttClient mqttClient,
    IPropertyInfoReader propertyInfoReader,
    ILogger logger)
    : StateEntityBase<ValveConfig>(config, entityUniqueId, deviceNodeId, mqttClient, propertyInfoReader, logger), IMqttSubscriber
{
    void IMqttSubscriber.OnReceived(ReceivedMqttMessage message)
    {
        // >>>> how? ... state is reported as open, opening, closed, closing. Command (received) is OPEN or CLOSE.
        //      how to do this with one property? Seems more like a property and a command method.
        //
        //      Attribute to pair a state property to command method?
        //
        //      [Dotnet2HassCommandMethod(statusProperty = nameof(StatusProperty)]
        //
        //      Or by declaration .... <<<< better 
        if (message.TopicAction == TopicAction.SetCmd)
        {
            //xx;
            // todo - use configured command method ... nameof(Zone.OperateValveCommand) ... support enums???
            //SetModelBoolProperty(message);  todo
        }
    }

    protected override EntityConfigMqttJsonBase GetConfigurationMqttPayload(DeviceConfig deviceConfig)
    {
        return new ValveConfigMqttJson(EntityUniqueId, Config, deviceConfig, MqttClient.ClientMqttId);
    }
}