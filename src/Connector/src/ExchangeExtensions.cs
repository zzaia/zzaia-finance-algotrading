using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.ExchangeAggregate;
using Zzaia.Finance.Exchange.Binance;
using Zzaia.Finance.Exchange.Ftx;
using Zzaia.Finance.Exchange.MercadoBitcoin;
using Zzaia.Finance.WebSocket;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Zzaia.Finance.Connector
{
    /// <summary>
    /// Extension methods for the Exchange clients.
    /// </summary>
    public static class ExchangeExtensions
    {
        /// <summary>
        /// Adds services and options for the exchange client.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="apiOptions">A delegate to configure the <see cref="ExchangeApiOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddExchange(this IServiceCollection services, ExchangeName exchangeName,
             Action<ClientCredential> privateCredential, Action<ClientCredential> tradeCredential)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (exchangeName is null)
            {
                throw new ArgumentNullException(nameof(exchangeName));
            }

            if (exchangeName.Equals(ExchangeName.MercadoBitcoin))
            {
                services.AddSingleton((s) =>
                {
                    var logger = (ILogger<MercadoBitcoinExchange>)s.GetService(typeof(ILogger<MercadoBitcoinExchange>));
                    var telemetry = (TelemetryClient)s.GetService(typeof(TelemetryClient));
                    var clientFactory = (IHttpClientFactory)s.GetService(typeof(IHttpClientFactory));
                    return new MercadoBitcoinExchange(privateCredential, tradeCredential, logger, telemetry, clientFactory);
                });
            }
            else if (exchangeName.Equals(ExchangeName.Binance))
            {
                services.AddSingleton<BinanceExchange>();
            }
            else if (exchangeName.Equals(ExchangeName.Ftx))
            {
                services.AddSingleton<IWebSocketClient, WebSocketClient>();
                services.AddSingleton((s) =>
                {
                    var logger = (ILogger<FtxExchange>)s.GetService(typeof(ILogger<FtxExchange>));
                    var telemetry = (TelemetryClient)s.GetService(typeof(TelemetryClient));
                    var clientFactory = (IHttpClientFactory)s.GetService(typeof(IHttpClientFactory));
                    var webSocketClient = (IWebSocketClient)s.GetService(typeof(IWebSocketClient));
                    return new FtxExchange(privateCredential, tradeCredential, logger, telemetry, clientFactory, webSocketClient);
                });
            }

            return services;
        }
    }
}