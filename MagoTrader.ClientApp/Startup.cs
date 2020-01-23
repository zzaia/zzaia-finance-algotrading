using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Web;
using MagoTrader.Core.Repositories;

namespace MagoTrader.ClientApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //-----------API Access------------------------------
            services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient(){ BaseAddress = new System.Uri("https://www.mercadobitcoin.net/api/") };
                return client;
            });
            services.AddScoped<IFetchDataService,FetchDataService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
