using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using Microsoft.Extensions.Logging;
using System;

namespace MarketIntelligency.EventManager
{
    public interface IDataStreamSource
    {
        /// <summary>
        /// Streams initial snapshot of the order book
        /// </summary>
        IObservable<EventSource<OrderBook>> OrderBookSnapshotStream { get; }

        /// <summary>
        /// Streams every update to the order book
        /// </summary>
        IObservable<EventSource<OrderBook>> OrderBookStream { get; }

        void Publish<T>(EventSource<T> eventSource, ILogger logger) where T : class;
        void Publish<T>(EventSource<T> eventSource) where T : class;
    }
}