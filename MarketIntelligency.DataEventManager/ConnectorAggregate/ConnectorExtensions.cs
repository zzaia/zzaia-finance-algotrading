using Microsoft.Extensions.DependencyInjection;
using System;

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
        public static IServiceCollection AddConnector(this IServiceCollection services, Action<ConnectorOptions> connectorOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //TODO: set a guid s reference for the singleton service, to be able to retrieve the connector options from a collection of options in the constructor.
            var connectorOptionsModel = new ConnectorOptions();
            connectorOptions.Invoke(connectorOptionsModel);
            var connectorInstance = new ConnectorService(connectorOptionsModel);
            services.AddSingleton(connectorInstance);
            return services;
        }
    }
}