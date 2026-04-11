using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoeticTools.Net2HassMqtt.Framework;

public static class DateTimeExtensions
{
    public static DateTime ToUtc(this DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
    {
        var utcDateTime = dateTime.ToUtc();
        return new DateTimeOffset(utcDateTime);
    }
}
