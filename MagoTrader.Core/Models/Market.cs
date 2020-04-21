using MagoTrader.Core.Exchange;
using System;

namespace MagoTrader.Core.Models
{
    public class Market
    {
        public Guid Id { get; set; }
        public string Ticker { get; }
        public AssetTickerEnum Base { get; }
        public AssetTickerEnum Main { get; }
        public Market( AssetTickerEnum mainTicker, AssetTickerEnum baseTicker )
        {
            Main = mainTicker;
            Base = baseTicker;
            Ticker = $"{mainTicker.ToString()}/{baseTicker.ToString()}";
        }
    }
}