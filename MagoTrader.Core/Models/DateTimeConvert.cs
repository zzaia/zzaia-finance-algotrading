using System;
using System.Globalization;

namespace MagoTrader.Core.Models
{
    public class DateTimeConvert
    {
        /// <summary>
        /// Returns the Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public static DateTimeOffset CurrentUtcDateTimeOffset() 
        {
            return DateTimeOffset.UtcNow; 
        }

        /// <summary>
        /// Returns the Coordinated Universal Time (UTC) timestamp in milliseconds.
        /// </summary>
        public static long CurrentUtcTimestamp()
        {
          return  DateTimeOffsetToTimestamp(CurrentUtcDateTimeOffset());
        }

        /// <summary>
        /// Returns the machine's local datetime.
        /// </summary>
        public static DateTimeOffset CurrentLocalDateTimeOffset()
        {
            return DateTimeOffset.Now;
        }

        /// <summary>
        /// Returns the machine's local timestamp in milliseconds.
        /// </summary>
        public static long CurrentLocalTimestamp()
        {
            return DateTimeOffsetToTimestamp(CurrentLocalDateTimeOffset());
        }

        /// <summary>
        /// Convert a datetime to Coordinated Universal Time (UTC) timestamp in milliseconds.
        /// </summary>
        public static long DateTimeToTimestamp(DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Convert a Coordinated Universal Time (UTC) datetime to a UTC timestamp in milliseconds.
        /// </summary>
        public static long DateTimeOffsetToTimestamp(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Convert a timestamp to a Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public static DateTimeOffset TimestampToDateTimeOffset(string timestamp, bool IsMillisecond)
        {
            return TimestampToDateTimeOffset(Convert.ToInt64(timestamp),IsMillisecond);
        }
         
        /// <summary>
        /// Convert a timestamp to a Coordinated Universal Time (UTC) datetime.
        /// </summary>
        public static DateTimeOffset TimestampToDateTimeOffset(long timestamp, bool IsMilliseconds)
        {
            return IsMilliseconds 
                ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToUniversalTime()
                : DateTimeOffset.FromUnixTimeSeconds(timestamp).ToUniversalTime();
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
