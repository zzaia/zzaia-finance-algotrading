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
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IFetchDataService FetchDataService { get; set; }

        protected override void OnInitialized()
        {

            //price = await FetchDataService.GetPricesAsync();
            //return base.OnInitializedAsync();
        }
    }
}
