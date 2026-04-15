namespace NoeticTools.Net2HassMqtt.Framework;

public static class DateTimeOffsetExtensions
{

    public static string ToIso8601String(this DateTimeOffset dateTime)
    {
        return dateTime.ToString("o");
    }
}