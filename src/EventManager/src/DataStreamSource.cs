﻿using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using Microsoft.Extensions.Logging;
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

        public void Publish<T>(EventSource<T> eventSource, ILogger logger) where T : class
        {
            logger.LogInformation($"### Publishing event with a {eventSource.Content.GetType().Name} ###");
            Publish(eventSource);
        }

        public void Publish<T>(EventSource<T> eventSource) where T : class
        {
            if (eventSource.Content.GetType() == typeof(OrderBook))
            {
                _orderBookSubject.OnNext(eventSource as EventSource<OrderBook>);
            }
        }

        public void Dispose()
        {
            _orderBookSubject.Dispose();
        }
    }
}
