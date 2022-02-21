using MarketIntelligency.Core.Models.ExchangeAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx.WebSockets
{
    internal interface IFtxWebSocketClient
    {
        Task ManageSubscribeTicker(List<string> oldList, List<string> newList, CancellationToken cancellationToken);
        Task SubscribeOrderbook(string pair, CancellationToken stoppingToken);
        Task UnsubscribeOrderbook(string pair, CancellationToken stoppingToken);
        Task AuthenticateSubscription(ClientCredential clientCredentials);
    }
}