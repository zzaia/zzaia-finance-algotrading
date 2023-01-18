using System;

namespace Zzaia.Finance.Core.Models.ExchangeAggregate
{
    public class ExchangeLimitRate
    {
        public bool UseTotal { get; set; }
        public double Rate { get; set; }
        public TimeSpan Period { get { return TimeSpan.FromSeconds(1 / Rate); } }
        public double Called { get; set; }
    }
}
