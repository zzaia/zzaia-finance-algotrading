using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Zzaia.Finance.EventManager
{
    public class DataStreamSource : IDataStreamSource
    {
        /// <summary>
        /// Use this subject to stream order book snapshot data
        /// </summary>
        private readonly Subject<EventSource<OrderBook>> _orderBookSubject = new();

        /// <inheritdoc />
        public IObservable<EventSource<OrderBook>> OrderBookStream => _orderBookSubject.AsObservable();

        public IConnectableObservable<EventSource<OrderBook>> ConnectedObservable => OrderBookStream.Replay(1000, TimeSpan.FromSeconds(60))
                                                                                                    .ObserveOn(Scheduler.Default)
            .Publish();

        public void Publish<T>(EventSource<T> eventContent) where T : class
        {
            if (eventContent.Content.GetType() == typeof(OrderBook))
            {
                var orderbook = eventContent.Content as OrderBook;
                var eventToPublish = new EventSource<OrderBook>(orderbook, eventContent.OcurredAt, eventContent.RecordedAt);
                _orderBookSubject.OnNext(eventToPublish);
            }
        }

        public void Dispose()
        {
            _orderBookSubject.Dispose();
        }
    }
}
