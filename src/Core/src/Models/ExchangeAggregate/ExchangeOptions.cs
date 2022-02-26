using System;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ExchangeOptions
    {
        public bool HasWebSocket { get; set; }
        public bool HasWebApi { get; set; }
        public TimeSpan ReconnectTimeSpan { get; set; }
    }
}