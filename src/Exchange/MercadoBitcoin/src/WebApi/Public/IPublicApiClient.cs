using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Public
{
    public interface IPublicApiClient
    {
        Task<Response<OHLCVDTO>> GetDaySummaryOHLCVAsync(string mainTicker, int year, int month, int day, CancellationToken cancellationToken);
        Task<Response<TickerDataDTO>> GetLast24hOHLCVAsync(string mainTicker, CancellationToken cancellationToken);
        Task<Response<OrderBookDTO>> GetOrderBookAsync(string mainTicker, CancellationToken cancellationToken);
        Task<Response<IEnumerable<TradeDTO>>> GetTradesSinceTIDAsync(string mainTicker, string tid, CancellationToken cancellationToken);
        Task<Response<IEnumerable<TradeDTO>>> GetLastTradesAsync(string mainTicker, CancellationToken cancellationToken);
        Task<Response<IEnumerable<TradeDTO>>> GetTradesBetweenTimeStampAsync(string mainTicker, int fromTimeStamp, int toTimeStamp, CancellationToken cancellationToken);
        Task<Response<IEnumerable<TradeDTO>>> GetTradesFromTimeStampAsync(string mainTicker, int timeStamp, CancellationToken cancellationToken);
    }
}