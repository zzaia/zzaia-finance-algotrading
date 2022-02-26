﻿using System;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ExchangeOptions
    {
        public bool HasWebSocket { get; set; }
        public bool HasWebApi { get; set; }
        public TimeSpan CheckForLivenessTimeSpan { get; set; }
    }
}