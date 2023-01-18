using System;

namespace Zzaia.Finance.Core.Utils
{
    /// <summary>
    /// DateTime utils to support high resolution
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Unix base datetime (1.1. 1970)
        /// </summary>
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
            return DateTimeOffsetToTimestamp(CurrentUtcDateTimeOffset());
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
            return TimestampToDateTimeOffset(Convert.ToInt64(timestamp), IsMillisecond);
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

        /// <summary>
        /// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static DateTime ConvertFromUnixSeconds(double timeInSec)
        {
            var unixTimeStampInTicks = (long)(timeInSec * TimeSpan.TicksPerSecond);
            return new DateTime(UnixBase.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static DateTime? ConvertFromUnixSeconds(double? timeInSec)
        {
            if (!timeInSec.HasValue)
                return null;
            return ConvertFromUnixSeconds(timeInSec.Value);
        }

        /// <summary>
        /// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static DateTime ConvertFromUnixSeconds(decimal timeInSec)
        {
            var unixTimeStampInTicks = (long)(timeInSec * TimeSpan.TicksPerSecond);
            return new DateTime(UnixBase.Ticks + unixTimeStampInTicks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Convert from unix seconds into DateTime with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static DateTime? ConvertFromUnixSeconds(decimal? timeInSec)
        {
            if (!timeInSec.HasValue)
                return null;
            return ConvertFromUnixSeconds(timeInSec.Value);
        }


        /// <summary>
        /// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static double ToUnixSeconds(this DateTime date)
        {
            var unixTimeStampInTicks = (date.ToUniversalTime() - UnixBase).Ticks;
            return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static double? ToUnixSeconds(this DateTime? date)
        {
            if (!date.HasValue)
                return null;
            return ToUnixSeconds(date.Value);
        }

        /// <summary>
        /// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static decimal ToUnixSecondsDecimal(this DateTime date)
        {
            var unixTimeStampInTicks = Convert.ToDecimal((date.ToUniversalTime() - UnixBase).Ticks);
            return unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Convert DateTime into unix seconds with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static decimal? ToUnixSecondsDecimal(this DateTime? date)
        {
            if (!date.HasValue)
                return null;
            return ToUnixSecondsDecimal(date.Value);
        }
        /// <summary>1
        /// 
        /// Convert DateTime into unix seconds string with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static string ToUnixSecondsString(this DateTime? value)
        {
            return value?.ToUnixSecondsString();
        }

        /// <summary>
        /// Convert DateTime into unix seconds string with high resolution (6 decimal places for milliseconds)
        /// </summary>
        public static string ToUnixSecondsString(this DateTime value)
        {
            var seconds = value.ToUnixSecondsDecimal();
            var str = seconds.ToString("0.000000");
            return str;
        }
    }
}
