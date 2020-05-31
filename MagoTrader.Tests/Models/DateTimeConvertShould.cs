﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MagoTrader.Core.Models;

namespace MagoTrader.Tests.Models
{
    public class DateTimeConvertShould
    {
        [Fact]
        public void ConvertTimestampMillisecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeConvert.CurrentUtcDateTimeOffset();
            var millisecondsTimestamp1 = DateTimeConvert.DateTimeOffsetToTimestamp(dateTimeOffset1);
            var dateTimeOffset2 = new DateTime(1970,4,24).ToUniversalTime();
            var millisecondsTimestamp2= DateTimeConvert.DateTimeToTimestamp(dateTimeOffset2);
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeConvert.TimestampToDateTimeOffset(millisecondsTimestamp1, true);
            DateTimeOffset result2 = DateTimeConvert.TimestampToDateTimeOffset(millisecondsTimestamp2, true);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime,tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }
        [Fact]
        public void ConvertTimestampSecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeConvert.CurrentUtcDateTimeOffset();
            var secondsTimestamp1 = dateTimeOffset1.ToUnixTimeSeconds();
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var secondsTimestamp2 = new DateTimeOffset(dateTimeOffset2).ToUnixTimeSeconds();
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeConvert.TimestampToDateTimeOffset(secondsTimestamp1, false);
            DateTimeOffset result2 = DateTimeConvert.TimestampToDateTimeOffset(secondsTimestamp2, false);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }

        [Fact]
        public void ConvertTimestampStringSecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeConvert.CurrentUtcDateTimeOffset();
            var secondsTimestamp1 = dateTimeOffset1.ToUnixTimeSeconds();
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var secondsTimestamp2 = new DateTimeOffset(dateTimeOffset2).ToUnixTimeSeconds();
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeConvert.TimestampToDateTimeOffset(secondsTimestamp1.ToString(), false);
            DateTimeOffset result2 = DateTimeConvert.TimestampToDateTimeOffset(secondsTimestamp2.ToString(), false);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }
        [Fact]
        public void ConvertTimestampStringMillisecondsToDateTimeOffset()
        {
            //Arrange:
            var dateTimeOffset1 = DateTimeConvert.CurrentUtcDateTimeOffset();
            var millisecondsTimestamp1 = DateTimeConvert.DateTimeOffsetToTimestamp(dateTimeOffset1);
            var dateTimeOffset2 = new DateTime(1970, 4, 24).ToUniversalTime();
            var millisecondsTimestamp2 = DateTimeConvert.DateTimeToTimestamp(dateTimeOffset2);
            var tolerance = TimeSpan.FromSeconds(1);

            //Act:
            DateTimeOffset result1 = DateTimeConvert.TimestampToDateTimeOffset(millisecondsTimestamp1.ToString(), true);
            DateTimeOffset result2 = DateTimeConvert.TimestampToDateTimeOffset(millisecondsTimestamp2.ToString(), true);

            //Assert:
            Assert.Equal(dateTimeOffset1.DateTime, result1.DateTime, tolerance);
            Assert.Equal(dateTimeOffset2, result2.DateTime, tolerance);

        }
    }
}