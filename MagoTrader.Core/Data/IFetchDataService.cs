using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagoTrader.Core.Models;

namespace MagoTrader.Core.Repositories
{
    public interface IFetchDataService
    {
        Task<OHLCV[]> GetForecastAsync(DateTime startDate);

    }

}