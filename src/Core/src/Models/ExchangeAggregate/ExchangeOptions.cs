using System;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ExchangeOptions
    {
        public bool HasWebSocket { get; set; }
        public bool HasWebApi { get; set; }
        /// <summary>
        /// Period in which check for websocket connection;
        /// </summary>
        public TimeSpan CheckForLivenessTimeSpan { get; set; }
    }
}