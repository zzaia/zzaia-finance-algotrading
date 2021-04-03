using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MarketIntelligency.EventManager
{
    public class StreamSource : IStreamSource, IDisposable
    {
        /// <summary>
        /// Use this subject to stream order book snapshot data
        /// </summary>
        private readonly Subject<EventSource<OrderBook>> _orderBookSnapshotSubject = new();

        /// <summary>
        /// Use this subject to stream order book data (level difference)
        /// </summary>
        private readonly Subject<EventSource<OrderBook>> _orderBookSubject = new();

        /// <inheritdoc />
        public IObservable<EventSource<OrderBook>> OrderBookSnapshotStream => _orderBookSnapshotSubject.AsObservable();

        /// <inheritdoc />
        public IObservable<EventSource<OrderBook>> OrderBookStream => _orderBookSubject.AsObservable();


        public void Publish<T>(EventSource<T> eventSource) where T : class
        {
            if (eventSource.Content.GetType() == typeof(OrderBook))
            {
                _orderBookSnapshotSubject.OnNext(eventSource as EventSource<OrderBook>);
            }
        }

        public void Dispose()
        {
            _orderBookSnapshotSubject.Dispose();
            _orderBookSubject.Dispose();
        }
    }
}
