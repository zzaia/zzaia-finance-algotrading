using System;
using System.Collections.Generic;
using System.Text;

namespace MarketMaker.Core.Models
{
    public class TimeFrame
    {
        public TimeFrame(TimeFrameEnum timeFrame)
        {
            Enum = timeFrame;
            TimeSpan = GetTimeSpanFromTimeFrame(timeFrame);
        }
        public TimeFrameEnum Enum { get; }
        public TimeSpan TimeSpan { get; }
        static public TimeSpan GetTimeSpanFromTimeFrame(TimeFrameEnum timeFrame)
        {
            var timeFrames = new Dictionary<TimeFrameEnum, TimeSpan>
            {
                { TimeFrameEnum.m1, TimeSpan.FromMinutes(1)},
                { TimeFrameEnum.m3, TimeSpan.FromMinutes(3)},
                { TimeFrameEnum.m5, TimeSpan.FromMinutes(5)},
                { TimeFrameEnum.m15, TimeSpan.FromMinutes(15)},
                { TimeFrameEnum.m30, TimeSpan.FromMinutes(30)},
                { TimeFrameEnum.H1, TimeSpan.FromHours(1)},
                { TimeFrameEnum.H2, TimeSpan.FromHours(2)},
                { TimeFrameEnum.H4, TimeSpan.FromHours(4)},
                { TimeFrameEnum.H6, TimeSpan.FromHours(6)},
                { TimeFrameEnum.H8, TimeSpan.FromHours(8)},
                { TimeFrameEnum.H12, TimeSpan.FromHours(12)},
                { TimeFrameEnum.D1, TimeSpan.FromDays(1)},
                { TimeFrameEnum.D3, TimeSpan.FromDays(3)},
                { TimeFrameEnum.W1, TimeSpan.FromDays(7)},
                { TimeFrameEnum.W2, TimeSpan.FromDays(7)},
                { TimeFrameEnum.M1, TimeSpan.FromDays(28) },
                { TimeFrameEnum.M2, TimeSpan.FromDays(28) }
            };
            return timeFrames[timeFrame];
        }
    }
}
