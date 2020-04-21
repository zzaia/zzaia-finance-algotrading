using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;

namespace MagoTrader.Services
{
    public class MockFetchDataService : IFetchDataService
    {
        public Task<OHLCV[]> GetDefaultDaySummaryAsync(DateTime startDate, ExchangeNameEnum exchangeName)
        {
            var rng = new Random();
            return  Task.FromResult(Enumerable.Range(1, 5).Select(index => new OHLCV
            {
                DateTime = startDate.AddDays(index),
                Open = rng.Next(100, 550000),
                High = rng.Next(100, 550000),
                Low = rng.Next(100, 550000),
                Close = rng.Next(100, 550000),
                Volume = rng.Next(100, 550000)
            }).ToArray());
        }

    }
}
