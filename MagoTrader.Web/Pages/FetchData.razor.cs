using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;
using MagoTrader.Data.Repositories;
namespace MagoTrader.Web.Pages
{
    public class FetchDataBase : ComponentBase
    {
        [Inject]
        public IFetchDataService FetchDataService { get; set; }
        /*
        [Inject]
        public IOHLCVRepository OHLCVRepository { get; set; }
        */
        protected OHLCV[] forecasts { get; set; }
        public Decimal[] prices { get; set; }
        protected override async Task OnInitializedAsync()
        {
            forecasts = await FetchDataService.GetForecastAsync(DateTime.Now);
            //prices = await FetchDataService.GetLastPriceStreamAsync();
            //return base.OnInitializedAsync();
        }
    }
}
