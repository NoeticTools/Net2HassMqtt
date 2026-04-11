namespace NoeticTools.Net2HassMqtt.Configuration;

public sealed partial class TimestampSensorConfig : SensorConfig
{
    protected override void CustomInit()
    {
        base.CustomInit();

        UnitOfMeasurement = null;
        Options = null;
    }
}