using Microsoft.Extensions.DependencyInjection;
using System;

namespace MagoTrader.Exchange
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

            services.Configure("KycApi", apiOptions);
            
            services.AddHttpClient<ApiClient>(c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClient-KycApiClient-3rz");
            });
            
            return services;
        }
    }
}
