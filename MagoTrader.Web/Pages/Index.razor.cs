using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MarketMaker.Core.Models;
using MarketMaker.Core.Repositories;
using MarketMaker.Data.Repositories;

namespace MarketMaker.Web.Pages
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
