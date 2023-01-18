using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using System;
using System.Reactive.Subjects;

namespace Zzaia.Finance.EventManager
{
    public interface IDataStreamSource
    {
        /// <summary>
        /// Streams initial snapshot of the order book
        /// </summary>
        IObservable<EventSource<OrderBook>> OrderBookStream { get; }
        IConnectableObservable<EventSource<OrderBook>> ConnectedObservable { get; }

        void Publish<T>(EventSource<T> eventSource) where T : class;
    }
}