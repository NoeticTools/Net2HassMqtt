![](/Documentation/Images/Net2HassMqtt_banner_820x70.png)

# Developer Secrets

The sample applications require the following secrets to run:

* MQTT broker's network address
* MQTT broker's username
* MQTT broker's password

The code obtains these using [Microsoft's secrets manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows#enable-secret-storage).
You will need to setup secrets using the keys:

* "MqttBroker:Address"
* "MqttBroker:Username"
* "MqttBroker:Password"

Instructions can be found [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows#enable-secret-storage).

You will end up with a secrets json file something like:

```json
{
  "MqttBroker:Address": "<IP_ADDRESS>",
  "MqttBroker:Username": "<USERNAME>",
  "MqttBroker:Password": "<PASSWORD>"
}
```

Sample code to read secrets:

```csharp
    var appConfig = new ConfigurationBuilder()
                    .AddUserSecrets<Program>()
                    .Build();

    var brokerAddress = appConfig["MqttBroker:Address"];
    var brokerUsername = appConfig["MqttBroker:Username"];
    var brokerPassword = appConfig["MqttBroker:Password"];
```