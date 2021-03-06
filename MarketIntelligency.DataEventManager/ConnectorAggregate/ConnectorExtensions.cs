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
        /// <param name="connectorOptions">A delegate to configure the connector opations<see cref="ConnectorOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddConnector(this IServiceCollection services, Action<ConnectorOptions> connectorOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton((s) =>
                {
                    var service = (ConnectorService)s.GetService(typeof(ConnectorService));
                    service.Configure(connectorOptions);
                    return service;
                });
            return services;
        }
    }
}