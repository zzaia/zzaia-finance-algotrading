using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using System;

namespace MarketIntelligency.EventManager
{
    public interface IStreamSource
    {
        /// <summary>
        /// Streams initial snapshot of the order book
        /// </summary>
        IObservable<EventSource<OrderBook>> OrderBookSnapshotStream { get; }

        /// <summary>
        /// Streams every update to the order book
        /// </summary>
        IObservable<EventSource<OrderBook>> OrderBookStream { get; }

        void Publish<T>(EventSource<T> eventSource) where T : class;
    }
}