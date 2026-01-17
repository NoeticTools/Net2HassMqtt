using NoeticTools.Net2HassMqtt.Framework;


namespace NoeticTools.Net2HassMqtt.Configuration;

public sealed partial class EnumSensorConfig : SensorConfig
{
    protected override void CustomInit()
    {
        base.CustomInit();

        UnitOfMeasurement = null; // see: https://www.home-assistant.io/integrations/sensor.mqtt/
        Options = [];
        var statusValueType = new PropertyInfoReader().GetPropertyGetterInfo(Model!, StatusPropertyName!)!.PropertyType;
        var enumNames = Enum.GetNames(statusValueType);
        foreach (var enumName in enumNames)
        {
            Options.Add(enumName);
        }
    }
}
