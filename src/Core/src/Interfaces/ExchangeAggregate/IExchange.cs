﻿using Crypto.Websocket.Extensions.Core.OrderBooks.Models;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Interfaces.ExchangeAggregate
{
    /// <summary>
    /// Describes all common public methods for all implemented exchanges under MarketIntelligency.Exchange namespace.
    /// </summary>
    public interface IExchange
    {
        static ExchangeInfo Information { get; }
        ExchangeInfo Info { get; }

        Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken);
        void SetOrderBookSubscription(Market market);
        void SubscribeToOrderBook(Action<IList<IOrderBookChangeInfo>> onNext);
    }
}