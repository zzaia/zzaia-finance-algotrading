using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using System;
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

        Task AuthenticateAsync(ClientCredential clientCredentials);
        Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken);
        Task SubscribeOrderbookAsync(Market market, CancellationToken cancellationToken);
        Task UnsubscribeOrderbookAsync(Market market, CancellationToken cancellationToken);
        Task ReceiveAsync(Action<OrderBook> action, CancellationToken cancellationToken);
        Task InitializeAsync(CancellationToken cancellationtoken);
        Task RestartAsync(CancellationToken cancellationToken);
        Task ConfirmLivenessAsync(CancellationToken stoppingToken);
    }
}