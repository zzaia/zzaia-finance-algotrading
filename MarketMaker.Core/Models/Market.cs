using System;

namespace MarketMaker.Core.Models
{
    public class Market
    {
        public Guid Id { get; set; }
        public string Ticker { get; }
        public AssetTicker Base { get; }
        public AssetTicker Main { get; }
        public Market(AssetTicker mainTicker, AssetTicker baseTicker)
        {
            Main = mainTicker;
            Base = baseTicker;
            Ticker = $"{mainTicker}/{baseTicker}";
        }
    }
}