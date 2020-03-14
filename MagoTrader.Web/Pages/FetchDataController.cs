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
    public class FetchDataController : ComponentBase
    {
        [Inject]
        public IFetchDataService FetchDataService { get; set; }
        /*
        [Inject]
        public IOHLCVRepository OHLCVRepository { get; set; }
        */
        public OHLCV[] forecasts { get; set; }
        protected override async Task OnInitializedAsync()
        {
            forecasts = await FetchDataService.GetForecastAsync(DateTime.Now);
            //return base.OnInitializedAsync();
        }
    }
}
