using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;

namespace MagoTrader.Exchange.Repositories
{
    public class FetchFakeDataService //: IFetchDataService
    {
        public Task<OHLCV[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return  Task.FromResult(Enumerable.Range(1, 5).Select(index => new OHLCV
            {
                DateTime = startDate.AddDays(index),
                Open = rng.Next(-20, 55),
                High = rng.Next(-20, 55),
                Low = rng.Next(-20, 55),
                Close = rng.Next(-20, 55),
                Volume = rng.Next(-20, 55)
            }).ToArray());
        }

    }
}
