
namespace NoeticTools.Net2HassMqtt.Framework;

public static class DateTimeExtensions
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static DateTime ToUtc(this DateTime dateTimeUtc)
    {
        return DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
    }

    /// <summary>
    /// Converts a DateTime to a DateTimeOffset in UTC. HASS requires timestamps to be in ISO 8601 format with time zone info,
    /// so we need to ensure the DateTime is treated as UTC when converting to a string.
    /// </summary>
    /// <param name="dateTimeUtc">UTC date and time to convert</param>
    /// <returns></returns>
    public static DateTimeOffset ToDateTimeOffset(this DateTime dateTimeUtc)
    {
        var utcDateTime = dateTimeUtc.ToUtc();
        return new DateTimeOffset(utcDateTime);
    }

    public static string ToIso8601String(this DateTime dateTimeUtc)
    {
        var utcDateTime = dateTimeUtc.ToDateTimeOffset();
        return utcDateTime.ToString("o");
    }
}