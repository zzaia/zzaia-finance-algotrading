using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.Models;
using MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Private
{
    public interface IPrivateApiClient
    {
        Task<Response<TAPResponse<SystemMessagesDTO>>> GetListOfSystemMessagesAsync(ClientCredential clientCredential, string level);
        Task<Response<TAPResponse<AccountInformationDTO>>> GetAccountInformationAsync(ClientCredential clientCredential);
        Task<Response<TAPResponse<OrderInformationDTO>>> GetOrderByIdAsync(ClientCredential clientCredential, int orderId, string tickerPair);
        Task<Response<TAPResponse<OrdersInformationDTO>>> GetListOfOrdersAsync(ClientCredential clientCredential, string tickerPair, string statusList, bool hasFills);
        Task<Response<TAPResponse<OrderbookInformationDTO>>> GetCompleteOrderBookByTickerPairAsync(ClientCredential clientCredential, string tickerPair, bool fullQuantity);
    }
}
