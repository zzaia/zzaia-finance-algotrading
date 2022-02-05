using MarketIntelligency.Core.Models.EnumerationAggregate;

namespace MarketIntelligency.Core.Models.MarketAgregate
{
    public class Market
    {
        public string Ticker { get; }
        public Asset Base { get; }
        public Asset Main { get; }
        public Market(Asset mainTicker, Asset baseTicker)
        {
            Main = mainTicker;
            Base = baseTicker;
            Ticker = $"{mainTicker.DisplayName}/{baseTicker.DisplayName}";
        }
    }
}