using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

using MagoTrader.Core.Exchange;
using System.Net.Http;

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
        /// <param name="apiOptions">A delegate to configure the <see cref="ExchangeApiOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddExchangeClient(this IServiceCollection services, ExchangeNameEnum exchangeName, 
             Action<AuthApiOptions> privateApiOptions, Action<AuthApiOptions> tradeApiOptions, Action<HttpClient> configureClient)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
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
            switch (exchangeName)
            {
                case ExchangeNameEnum.MercadoBitcoin:
                    
                    services.AddHttpClient<MagoTrader.Exchange.MercadoBitcoin.Public.PublicApiClient>(configureClient);
                    if (privateApiOptions != null)
                    {
                        services.AddHttpClient<MagoTrader.Exchange.MercadoBitcoin.Private.PrivateApiClient>(configureClient);
                        services.Configure("MercadoBitcoinPrivateApiOptions", privateApiOptions);     
                    }
                    if (tradeApiOptions != null)
                    {
                        services.AddHttpClient<MagoTrader.Exchange.MercadoBitcoin.Trade.TradeApiClient>(configureClient);
                        services.Configure("MercadoBitcoinTradeApiOptions", tradeApiOptions);
                    }
                    services.AddScoped<Exchange.MercadoBitcoin.MercadoBitcoinExchange>();
                    break;
            }
            
            services.AddScoped<IExchangeSelector,ExchangeSelector>();
            return services;
        }
    }
}
