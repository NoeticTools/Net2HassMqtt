
namespace NoeticTools.Net2HassMqtt.Framework;

public static class DateTimeExtensions
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static DateTime ToUtc(this DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
    {
        var utcDateTime = dateTime.ToUtc();
        return new DateTimeOffset(utcDateTime);
    }

    public static string ToIso8601String(this DateTime dateTime)
    {
        var utcDateTime = dateTime.ToDateTimeOffset();
        return utcDateTime.ToString("o");
    }
}
