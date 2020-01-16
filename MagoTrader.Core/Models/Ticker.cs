using System;

namespace MagoTrader.Core.Models
{
    public class Ticker
    {
        public string String { get; set; }
        public AssetTicker Base { get; set; }
        public AssetTicker Main { get; set; }
    }
}