using System;

namespace MagoTrader.Core
{
    public class TimeLog
    {
        private DateTime _datetime;
        public DateTime CurrentDateTime{
            get { return _datetime; }
            set { _datetime = DateTime.UtcNow; }
        }
        
        private Int32 _timestamp;
        public Int32 CurrentTimestamp
        {
            get { return _timestamp ; }
            set { _timestamp = DateTimeToTimestamp(CurrentDateTime);}
        }

        public Int32 DateTimeToTimestamp(DateTime dt)
        {
            return Convert.ToInt32(new DateTimeOffset(dt).ToUnixTimeMilliseconds());
        }

    }
}
