using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MarketIntelligency.EventManager
{
    public class DataStreamSource : IDataStreamSource
    {
        /// <summary>
        /// Use this subject to stream order book snapshot data
        /// </summary>
        private readonly Subject<EventSource<OrderBook>> _orderBookSubject = new();

        /// <inheritdoc />
        public IObservable<EventSource<OrderBook>> OrderBookStream => _orderBookSubject.AsObservable();

        public void Publish<T>(EventSource<T> eventContent) where T : class
        {
            if (eventContent.Content.GetType() == typeof(OrderBook))
            {
                var orderbook = eventContent.Content as OrderBook;
                var eventToPublish = new EventSource<OrderBook>()
                {
                    Content = orderbook,
                    OcurredAt = eventContent.OcurredAt,
                    RecordedAt = eventContent.RecordedAt,
                };
                _orderBookSubject.OnNext(eventToPublish);
            }
        }

        public void Dispose()
        {
            _orderBookSubject.Dispose();
        }
    }
}
