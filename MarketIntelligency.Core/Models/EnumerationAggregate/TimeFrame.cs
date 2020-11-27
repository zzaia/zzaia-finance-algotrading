using System;

namespace MarketIntelligency.Core.Models.EnumerationAggregate
{    
    /*  ========================================================================================================
        System Default TimeFrames
        ======================================================================================================= */
    public class TimeFrame : Enumeration
    {
        public static readonly TimeFrame m1 = new TimeFrame(1, "1m", TimeSpan.FromMinutes(1));
        public static readonly TimeFrame m2 = new TimeFrame(2, "2m", TimeSpan.FromMinutes(2));
        public static readonly TimeFrame m3 = new TimeFrame(3, "3m", TimeSpan.FromMinutes(3));
        public static readonly TimeFrame m4 = new TimeFrame(4, "4m", TimeSpan.FromMinutes(4));
        public static readonly TimeFrame m5 = new TimeFrame(5, "5m", TimeSpan.FromMinutes(5));
        public static readonly TimeFrame m15 = new TimeFrame(6, "15m", TimeSpan.FromMinutes(15));
        public static readonly TimeFrame m30 = new TimeFrame(7, "30m", TimeSpan.FromMinutes(30));
        public static readonly TimeFrame H1 = new TimeFrame(8, "1H", TimeSpan.FromHours(1));
        public static readonly TimeFrame H2 = new TimeFrame(9, "2H", TimeSpan.FromHours(2));
        public static readonly TimeFrame H4 = new TimeFrame(10, "4H", TimeSpan.FromHours(4));
        public static readonly TimeFrame H6 = new TimeFrame(11, "6H", TimeSpan.FromHours(6));
        public static readonly TimeFrame H8 = new TimeFrame(12, "8H", TimeSpan.FromHours(8));
        public static readonly TimeFrame H12 = new TimeFrame(13, "12H", TimeSpan.FromHours(12));
        public static readonly TimeFrame D1 = new TimeFrame(14, "1D", TimeSpan.FromDays(1));
        public static readonly TimeFrame D2 = new TimeFrame(15, "2D", TimeSpan.FromDays(2));
        public static readonly TimeFrame D3 = new TimeFrame(16, "3D", TimeSpan.FromDays(3));
        public static readonly TimeFrame W1 = new TimeFrame(17, "1W", TimeSpan.FromDays(7));
        public static readonly TimeFrame W2 = new TimeFrame(18, "2W", TimeSpan.FromDays(14));
        public static readonly TimeFrame M1 = new TimeFrame(19, "1M", TimeSpan.FromDays(28));
        public static readonly TimeFrame M2 = new TimeFrame(20, "2M", TimeSpan.FromDays(56));
        public TimeFrame() { }
        private TimeFrame(int value, string displayName, TimeSpan timeSpan) : base(value, displayName) { _timeSpan = timeSpan; }

        private readonly TimeSpan _timeSpan;
        public TimeSpan TimeSpan
        {
            get { return _timeSpan; }
        }
    }
}
