using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarketIntelligency.Exchange.Binance.WebSocket
{
    /// <summary>
    /// Extension methods for the Exchange clients.
    /// </summary>
    public static class BinanceWebSocketExtensions
    {
        /// <summary>
        /// Adds services and options for the exchange client.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="apiOptions">A delegate to configure the <see cref="ExchangeApiOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddBinanceWebSocket(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<BinanceExchange>();

            return services;
        }
    }
}
