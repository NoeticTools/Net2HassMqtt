namespace Net2HassMqtt.Tests.ComponentTests.Framework.MessageMatching;

public abstract class MessagesBase(
    string clientId,
    string deviceId,
    string deviceName,
    string nodeId,
    string nodeName,
    string domainName,
    string deviceClass)
{
    protected string StateTopic => $"{clientId}/{deviceId}/{nodeId}";

    public virtual MessageMatcher Config =>
        GetConfigurationMessageWithoutOptions();

    protected MessageMatcher GetConfigurationMessageWithoutOptions(string? options = null)
    {
        options = options == null ? "" : $"""
                                          
                                            "options": "{options}",
                                          """;

        return new MessageMatcher($"homeassistant/{domainName}/{deviceId}/{deviceId}_{nodeId}/config",
                                  $$$"""
                                     {
                                       "device_class": "{{{deviceClass}}}",{{{options}}}
                                       "unit_of_measurement": "None",
                                       "retain": true,
                                       "availability": {
                                         "payload_available": "online",
                                         "payload_not_available": "offline",
                                         "topic": "{{{clientId}}}/bridge/state",
                                         "value_template": "{{ value_json.state }}"
                                       },
                                       "device": {
                                         "identifiers": [
                                           "{{{deviceId}}}"
                                         ],
                                         "manufacturer": "",
                                         "model": "",
                                         "name": "{{{deviceName}}}"
                                       },
                                       "json_attributes_template": "{{ value_json.attributes | tojson }}",
                                       "json_attributes_topic": "net2hassmqtt_test_start/{{{deviceId}}}/{{{nodeId}}}",
                                       "name": "{{{nodeName}}}",
                                       "object_id": "{{{deviceId}}}_{{{nodeId}}}",
                                       "state_topic": "net2hassmqtt_test_start/{{{deviceId}}}/{{{nodeId}}}",
                                       "unique_id": "{{{deviceId}}}_{{{nodeId}}}",
                                       "value_template": "{{ value_json.state }}"
                                     }
                                     """);
    }
}