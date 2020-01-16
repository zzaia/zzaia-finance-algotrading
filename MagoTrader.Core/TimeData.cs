using System;

namespace MagoTrader.Core.Models
{
    public class TimeData
    {
        
        public DateTime CurrentDateTime 
        {
            get { return DateTime.UtcNow; }
        }
        public Int32 CurrentTimestamp
        {
            get { return  DateTimeToTimestamp(CurrentDateTime); }
        }
       // public Int32 ToTimestamp{
       //     get{return Convert.ToInt32(new DateTimeOffset(_datetime).ToUnixTimeMilliseconds());}
       // }
        public static Int32 DateTimeToTimestamp(DateTime dt)
        {
            return Convert.ToInt32(new DateTimeOffset(dt).ToUnixTimeMilliseconds());
        }

    }
}
