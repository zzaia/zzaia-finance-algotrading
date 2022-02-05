using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Trade
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
    }
}