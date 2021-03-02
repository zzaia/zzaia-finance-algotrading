using MarketIntelligency.Core.Models.EnumerationAggregate;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketIntelligency.DataEventManager.ConnectorAggregate
{
    /// <summary>
    /// Extension methods for the Exchange clients.
    /// </summary>
    public static class ConnectorExtensions
    {
        /// <summary>
        /// Adds services and options for the connector.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="connectorOptions">A delegate to configure the <see cref="ConnectorOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddConnector(this IServiceCollection services, ExchangeName exchangeName, Action<ConnectorOptions> connectorOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (exchangeName is null)
            {
                throw new ArgumentNullException(nameof(exchangeName));
            }

            //TODO: set a guid s reference for the singleton service, to be able to retrieve the connector options from a collection of options in the constructor.

            if (exchangeName.Equals(ExchangeName.MercadoBitcoin))
            {
                services.AddSingleton<ConnectorService>("Nome01");
            }
            return services;
        }
    }
}