namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ExchangeLimitRate
    {
        public bool UseTotal { get; set; }
        public ExchangeLimitCalled Public { get; set; }
        public ExchangeLimitCalled Private { get; set; }
        public ExchangeLimitCalled Trade { get; set; }
        public ExchangeLimitCalled Total { get; set; }
    }
}
