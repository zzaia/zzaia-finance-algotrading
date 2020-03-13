using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;


using MagoTrader.Exchange;
using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;

namespace MagoTrader.Web
{    
    /// <summary>
    /// Extension methods for the Exchange clients.
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Adds services and options for the exchange client.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="apiOptions">A delegate to configure the <see cref="ApiOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddExchangeClient(this IServiceCollection services, Action<ApiOptions> apiOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (apiOptions is null)
            {
                throw new ArgumentNullException(nameof(apiOptions));
            }

            var a = new ApiOptions();
            apiOptions(a); 
            
            services.Configure($"{a.Name.ToString()}ApiClientOptions", apiOptions);     

            switch(a.Name)
            {
                case ExchangeName.MercadoBitcoin:
                    if(a.Authentication is null)
                    {
                        services.AddHttpClient<Exchange.MercadoBitcoin.PublicApiClient>(c =>
                        {
                            c.DefaultRequestHeaders.Add("Accept", "application/json");
                            c.DefaultRequestHeaders.Add("User-Agent", $"HttpClient-{a.Name.ToString()}ApiClient-MagoTrader");
                        });
                    }
                    else
                    {
                        services.AddHttpClient<Exchange.MercadoBitcoin.PrivateApiClient>(c =>
                        {
                            c.DefaultRequestHeaders.Add("Accept", "application/json");
                            c.DefaultRequestHeaders.Add("User-Agent", $"HttpClient-{a.Name.ToString()}ApiClient-MagoTrader");
                        });
                    }
                    break;
            }
                       
            return services;
        }
    }
}
