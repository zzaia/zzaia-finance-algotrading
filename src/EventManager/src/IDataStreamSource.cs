using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using System;

namespace MarketIntelligency.EventManager
{
    public interface IDataStreamSource
    {
        /// <summary>
        /// Streams initial snapshot of the order book
        /// </summary>
        IObservable<EventSource<OrderBook>> OrderBookStream { get; }

        void Publish<T>(EventSource<T> eventSource) where T : class;
    }
}