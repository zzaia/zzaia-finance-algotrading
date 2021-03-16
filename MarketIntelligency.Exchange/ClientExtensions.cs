using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace MarketIntelligency.Exchange
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
        /// <param name="apiOptions">A delegate to configure the <see cref="ExchangeApiOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddExchangeClient(this IServiceCollection services, ExchangeName exchangeName,
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

            /* // Typed http client by reflection (Runtime)
             * https://stackoverflow.com/questions/232535/how-do-i-use-reflection-to-call-a-generic-method
             *  Type myType = Type.GetType(
                String.Format("{0}.{1}.{1}",
                this.GetType().Namespace,
                exchangeName.ToString()));
                if (type == null) { 
                    var errorMessage = $"'{type}' is not a valid exchange type.";
                    _logger.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
             *  MethodInfo method = typeof(services).GetMethod("AddHttpClient");
                MethodInfo generic = method.MakeGenericMethod(myType);
                generic.Invoke(this, configureClient);
             */

            /*
            // Named http client
            services.AddHttpClient($"{exchangeName.ToString()}PublicApi", configureClient);
            if (privateApiOptions != null)
            {
                services.AddHttpClient($"{exchangeName.ToString()}PrivateApi", configureClient);
                services.Configure($"{exchangeName.ToString()}PrivateApiOptions", privateApiOptions);
            }
            if (tradeApiOptions != null)
            {
                services.AddHttpClient($"{exchangeName.ToString()}TradeApi", configureClient);
                services.Configure($"{exchangeName.ToString()}TradeApiOptions", tradeApiOptions);
            }
            */

            // Typed http client (Compile Time)
            if (exchangeName.Equals(ExchangeName.MercadoBitcoin))
            {
                services.AddSingleton((s) =>
                {
                    var logger = (ILogger<MercadoBitcoinExchange>)s.GetService(typeof(ILogger<MercadoBitcoinExchange>));
                    var telemetry = (TelemetryClient)s.GetService(typeof(TelemetryClient));
                    var client = (HttpClient)s.GetService(typeof(HttpClient));
                    return new MercadoBitcoinExchange(privateCredential, tradeCredential, logger, telemetry, client);
                });
            }

            return services;
        }
    }
}