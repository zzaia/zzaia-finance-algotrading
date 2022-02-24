using MarketIntelligency.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx
{
    public interface IFtxExchange
    {
        Task UnsubscribeOrderbookAsync(Market market, CancellationToken stoppingToken);
    }
}
