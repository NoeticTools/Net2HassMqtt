namespace NoeticTools.Net2HassMqtt.Configuration;

public partial class DateSensorConfig
{
    protected override void CustomInit()
    {
        base.CustomInit();
        
        UnitOfMeasurement = null;
        Options = null;
    }
}
