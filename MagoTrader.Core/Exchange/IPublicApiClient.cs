using MagoTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MagoTrader.Core.Exchange
{
    public interface IPublicApiClient
    {
        Task<OHLCV> GetDaySummaryOHLCVAsync(AssetTickerEnum ticker, DateTime dt);

    }
}
