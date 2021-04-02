using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.Models;
using MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Trade
{
    public interface ITradeApiClient
    {
        Task<Response<TAPResponse<OrderDTO>>> PlaceMarketBuyOrderAsync(ClientCredential clientCredential, string tickerPair, string cost, CancellationToken cancellationToken);
        Task<Response<TAPResponse<OrderDTO>>> PlaceMarketSellOrderAsync(ClientCredential clientCredential, string tickerPair, string quantity, CancellationToken cancellationToken);
        Task<Response<TAPResponse<OrderDTO>>> CancelOrderAsync(ClientCredential clientCredential, string tickerPair, int orderId, CancellationToken cancellationToken);
        Task<Response<TAPResponse<WithdrawalDTO>>> GetWithdrawalAsync(ClientCredential clientCredential, string ticker, int withdrawalId, CancellationToken cancellationToken);
        Task<Response<TAPResponse<WithdrawalDTO>>> PlaceWithdrawalAsync(ClientCredential clientCredential, string ticker, string walletAddress,
                                                                                                                                       string quantity,
                                                                                                                                       string transactionFee,
                                                                                                                                       CancellationToken cancellationToken,
                                                                                                                                       int destinationTag = int.MinValue,
                                                                                                                                       bool isAggregate = false,
                                                                                                                                       bool inBlockchain = false);
        Task<Response<TAPResponse<WithdrawalDTO>>> PlaceWithdrawalAsync(ClientCredential clientCredential, string accountRef, string quantity, CancellationToken cancellationToken);
        Task<Response<TAPResponse<T>>> PostSuppreme<T>(ClientCredential clientCredential, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken);
    }
}