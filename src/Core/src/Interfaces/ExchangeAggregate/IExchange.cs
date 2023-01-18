using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.ExchangeAggregate;
using Zzaia.Finance.Core.Models.MarketAgregate;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zzaia.Finance.Core.Interfaces.ExchangeAggregate
{
    /// <summary>
    /// Describes all common public methods for all implemented exchanges under Zzaia.Finance.Exchange namespace.
    /// </summary>
    public interface IExchange
    {
        static ExchangeInfo Information { get; }
        ExchangeInfo Info { get; }

        Task AuthenticateAsync(ClientCredential clientCredentials);
        Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken);
        Task SubscribeOrderbookAsync(Market market, CancellationToken cancellationToken);
        Task UnsubscribeOrderbookAsync(Market market, CancellationToken cancellationToken);
        Task ReceiveAsync(Action<dynamic> action, CancellationToken cancellationToken);
        Task InitializeAsync(CancellationToken cancellationtoken);
        Task RestartAsync(CancellationToken cancellationToken);
        Task ConfirmLivenessAsync(CancellationToken stoppingToken);
    }
}