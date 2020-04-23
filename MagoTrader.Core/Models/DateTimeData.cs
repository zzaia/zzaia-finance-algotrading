using System;
using System.Globalization;

namespace MagoTrader.Core.Models
{
    public class DateTimeData
    {
        /// <summary>
        /// Returns the Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public DateTimeOffset CurrentUtcDateTimeOffset 
        {
            get { return DateTimeOffset.UtcNow; }
        }

        /// <summary>
        /// Returns the Coordinated Universal Time (UTC) timestamp.
        /// </summary>
        public Int32 CurrentUtcTimestamp
        {
            get { return  DateTimeOffsetToTimestamp(CurrentUtcDateTimeOffset); }
        }

        /// <summary>
        /// Returns the machine's local datetime.
        /// </summary>
        public DateTimeOffset CurrentLocalDateTimeOffset
        {
            get { return DateTimeOffset.Now; }
        }

        /// <summary>
        /// Returns the machine's local timestamp.
        /// </summary>
        public Int32 CurrentLocalTimestamp
        {
            get { return DateTimeOffsetToTimestamp(CurrentLocalDateTimeOffset); }
        }

        /// <summary>
        /// Convert a datetime to Coordinated Universal Time (UTC) timestamp.
        /// </summary>
        public static Int32 DateTimeToTimestamp(DateTime dt)
        {
            return Convert.ToInt32(new DateTimeOffset(dt).ToUnixTimeMilliseconds());
        }

        /// <summary>
        /// Convert a Coordinated Universal Time (UTC) datetime to a UTC timestamp
        /// </summary>
        public static Int32 DateTimeOffsetToTimestamp(DateTimeOffset dateTimeOffset)
        {
            return Convert.ToInt32(dateTimeOffset.ToUnixTimeMilliseconds());
        }

        /// <summary>
        /// Convert a timestamp to a Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public static DateTimeOffset TimestampToDateTimeOffset(string timestamp)
        {
            return TimestampToDateTimeOffset(Convert.ToInt32(timestamp));
        }
         
        /// <summary>
        /// Convert a timestamp to a Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public static DateTimeOffset TimestampToDateTimeOffset(Int32 timestamp)
        {
            var timestampNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return  timestampNow % timestamp > 0
                ? DateTimeOffset.FromUnixTimeSeconds(timestamp).ToUniversalTime()
                : DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToUniversalTime();
        }

        /*
        /// <summary>
        /// Convert a local timestamp to a Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public static DateTimeOffset TimestampToDateTimeOffset(string timeStamp, CultureInfo cultureInfo)
        {
            var temp = TimestampToDateTimeOffset(timeStamp).ToLocalTime(cultureInfo);
            return DateTimeOffset.Parse(temp,cultureInfo.DateTimeFormat);
        }
        */
    }
}
