using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace WebSite.Helpers
{
    public static class DateTimeHelper
    {

        static DateTimeHelper()
        {
            var timeZoneOffsetString = ConfigurationManager.AppSettings["TimeZoneOffset"];
            var timeZoneOffset = 0;
            if (int.TryParse(timeZoneOffsetString, out timeZoneOffset))
            {
                TimeZoneOffset = timeZoneOffset;
            }
        }

        public static int TimeZoneOffset { get; private set; }

        public static DateTime ToDefaultTargetGmtTime(this DateTime dateTime)
        {
            var gmtTime = dateTime.ToUniversalTime();
            var targetTime = gmtTime.AddHours(TimeZoneOffset);
            return targetTime;
        }

        public static string ToMonthYearString(this DateTime dateTime)
        {
            var result = dateTime.ToString("MM/yyyy", CultureInfo.InvariantCulture);
            result = result.Replace(".", "");
            result = result.First().ToString().ToUpper() + result.Substring(1);
            return result;
        }

    }
}