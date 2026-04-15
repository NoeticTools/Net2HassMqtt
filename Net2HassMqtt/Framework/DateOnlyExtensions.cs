namespace NoeticTools.Net2HassMqtt.Framework;

public static class DateOnlyExtensions
{
    /// <summary>
    /// Converts a DateOnly to an ISO 8601 timestamp string with time set to midnight. HASS requires dates to be in ISO 8601 format with a time zone.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToIso8601String(this DateOnly date)
    {
        return new DateTime(date.Year, date.Month, date.Day).ToIso8601String();
    }
}