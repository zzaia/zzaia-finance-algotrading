using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;
using MagoTrader.Data.Repositories;
using MagoTrader.Core.Exchange;

namespace MagoTrader.Web.Pages
{
    public class DaySummaryBase : ComponentBase
    {
        [Inject] public IFetchDataService FetchDataService { get; set; }

        protected OHLCV[] Forecasts { get; set; }
        public Decimal[] Prices { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Forecasts = await FetchDataService.GetDefaultDaySummaryAsync(DateTimeOffset.Now.Subtract(TimeSpan.FromDays(1)), ExchangeNameEnum.MercadoBitcoin).ConfigureAwait(true);
        }
    }
}
