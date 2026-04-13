namespace Net2HassMqtt.Tests.ComponentTests.Framework.MqttMessageMatching;

public abstract class MqttMessageMatcherBase(
    string clientId,
    string deviceId,
    string deviceName,
    string nodeId,
    string nodeName,
    string domainName,
    string deviceClass,
    string? unitOfMeasurementOrNone = "None")
{
    public virtual MessageMatcher Config => GetConfigurationMessage(null, unitOfMeasurementOrNone);

    protected string StateTopic => $"{clientId}/{deviceId}/{nodeId}";

    protected MessageMatcher GetConfigurationMessage(string? options = null, string? unitOfMeasurement = null)
    {
        options = options == null
            ? ""
            : $"""
               
                 "options": {options},
               """;

        unitOfMeasurement = string.IsNullOrEmpty(unitOfMeasurementOrNone)
            ? ""
            : $"""
               
                 "unit_of_measurement": "{unitOfMeasurementOrNone}",
               """;
        //options = options == null
        //    ? """

        //        "unit_of_measurement": "None",
        //      """
        //    : $"""

        //         "options": {options},
        //       """;

        return new MessageMatcher($"homeassistant/{domainName}/{deviceId}/{deviceId}_{nodeId}/config",
                                  $$$"""
                                     {
                                       "device_class": "{{{deviceClass}}}",{{{unitOfMeasurement}}}{{{options}}}
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
                                       "default_entity_id": "{{{domainName}}}.{{{deviceId}}}_{{{nodeId}}}",
                                       "state_topic": "net2hassmqtt_test_start/{{{deviceId}}}/{{{nodeId}}}",
                                       "unique_id": "{{{deviceId}}}_{{{nodeId}}}",
                                       "value_template": "{{ value_json.state }}"
                                     }
                                     """);
    }
}