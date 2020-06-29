using MagoTrader.Core.Exchange;
using MagoTrader.Exchange.MercadoBitcoin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    public interface IPublicApiClient
    {
        Task<Response<OHLCVDTO>> GetDaySummaryOHLCVAsync(string mainTicker, int year, int month, int day);
        Task<Response<TickerDataDTO>> GetLast24hOHLCVAsync(string mainTicker);
        Task<Response<OrderBookDTO>> GetOrderBookAsync(string mainTicker);
        Task<Response<IEnumerable<TradeDTO>>> GetTradesSinceTIDAsync(string mainTicker, string tid);
        Task<Response<IEnumerable<TradeDTO>>> GetLastTradesAsync(string mainTicker);
        Task<Response<IEnumerable<TradeDTO>>> GetTradesBetweenTimeStampAsync(string mainTicker, int fromTimeStamp, int toTimeStamp);
        Task<Response<IEnumerable<TradeDTO>>> GetTradesFromTimeStampAsync(string mainTicker, int timeStamp);
    }
}