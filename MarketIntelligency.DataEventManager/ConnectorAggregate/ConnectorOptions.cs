using MarketIntelligency.Core.Models.EnumerationAggregate;
using System;

namespace MarketIntelligency.DataEventManager.ConnectorAggregate
{
    public class ConnectorOptions
    {
        public TimeFrame TimeFrame { get; set; }
        public Type DataIn { get; set; }
        public Type DataOut { get; set; }
    }
}