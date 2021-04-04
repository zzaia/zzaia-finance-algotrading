using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Private
{
    public interface IPrivateApiClient
    {
        Task<Response<TAPResponse<SystemMessagesDTO>>> GetListOfSystemMessagesAsync(ClientCredential clientCredential, string level, CancellationToken cancellationToken);
        Task<Response<TAPResponse<AccountInformationDTO>>> GetAccountInformationAsync(ClientCredential clientCredential, CancellationToken cancellationToken);
        Task<Response<TAPResponse<OrderInformationDTO>>> GetOrderByIdAsync(ClientCredential clientCredential, int orderId, string tickerPair, CancellationToken cancellationToken);
        Task<Response<TAPResponse<OrdersInformationDTO>>> GetListOfOrdersAsync(ClientCredential clientCredential, string tickerPair, string statusList, bool hasFills, CancellationToken cancellationToken);
        Task<Response<TAPResponse<OrderbookInformationDTO>>> GetCompleteOrderBookByTickerPairAsync(ClientCredential clientCredential, string tickerPair, bool fullQuantity, CancellationToken cancellationToken);
    }
}