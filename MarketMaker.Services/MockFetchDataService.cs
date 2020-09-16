using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MarketMaker.Core.Exchange;
using MarketMaker.Core.Models;
using MarketMaker.Core.Repositories;

namespace MarketMaker.Services
{
    public class MockFetchDataService : IFetchDataService
    {
        public Task<OHLCV[]> GetDefaultDaySummaryAsync(DateTimeOffset startDate, ExchangeNameEnum exchangeName)
        {
            var rng = new Random();
            return  Task.FromResult(Enumerable.Range(1, 5).Select(index => new OHLCV
            {
                DateTimeOffset = startDate.AddDays(index),
                Open = rng.Next(100, 550000),
                High = rng.Next(100, 550000),
                Low = rng.Next(100, 550000),
                Close = rng.Next(100, 550000),
                Volume = rng.Next(100, 550000)
            }).ToArray());
        }

    }
}
