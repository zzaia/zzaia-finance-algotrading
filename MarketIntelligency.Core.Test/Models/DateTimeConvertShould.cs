using MarketIntelligency.Core.Utils;
using System;
using Xunit;

namespace MarketIntelligency.Test.Models
{
    public class DateTimeConvertShould
    {
        [Fact]
        public void ConvertTimestampMillisecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeUtils.CurrentUtcDateTimeOffset();
            var millisecondsTimestamp1 = DateTimeUtils.DateTimeOffsetToTimestamp(dateTimeOffset1);
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var millisecondsTimestamp2 = DateTimeUtils.DateTimeToTimestamp(dateTimeOffset2);
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeUtils.TimestampToDateTimeOffset(millisecondsTimestamp1, true);
            DateTimeOffset result2 = DateTimeUtils.TimestampToDateTimeOffset(millisecondsTimestamp2, true);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }
        [Fact]
        public void ConvertTimestampSecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeUtils.CurrentUtcDateTimeOffset();
            var secondsTimestamp1 = dateTimeOffset1.ToUnixTimeSeconds();
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var secondsTimestamp2 = new DateTimeOffset(dateTimeOffset2).ToUnixTimeSeconds();
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeUtils.TimestampToDateTimeOffset(secondsTimestamp1, false);
            DateTimeOffset result2 = DateTimeUtils.TimestampToDateTimeOffset(secondsTimestamp2, false);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }

        [Fact]
        public void ConvertTimestampStringSecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeUtils.CurrentUtcDateTimeOffset();
            var secondsTimestamp1 = dateTimeOffset1.ToUnixTimeSeconds();
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var secondsTimestamp2 = new DateTimeOffset(dateTimeOffset2).ToUnixTimeSeconds();
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeUtils.TimestampToDateTimeOffset(secondsTimestamp1.ToString(), false);
            DateTimeOffset result2 = DateTimeUtils.TimestampToDateTimeOffset(secondsTimestamp2.ToString(), false);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }
        [Fact]
        public void ConvertTimestampStringMillisecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeUtils.CurrentUtcDateTimeOffset();
            var millisecondsTimestamp1 = DateTimeUtils.DateTimeOffsetToTimestamp(dateTimeOffset1);
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var millisecondsTimestamp2 = DateTimeUtils.DateTimeToTimestamp(dateTimeOffset2);
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeUtils.TimestampToDateTimeOffset(millisecondsTimestamp1.ToString(), true);
            DateTimeOffset result2 = DateTimeUtils.TimestampToDateTimeOffset(millisecondsTimestamp2.ToString(), true);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }
    }
}
