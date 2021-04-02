using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MarketIntelligency.Connector
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
            services.AddHostedService((s) =>
                    {
                        var mediator = (IMediator)s.GetService(typeof(IMediator));
                        var exchangeSelector = (IExchangeSelector)s.GetService(typeof(IExchangeSelector));
                        var logger = (ILogger<ConnectorProcessor>)s.GetService(typeof(ILogger<ConnectorProcessor>));
                        var telemetry = (TelemetryClient)s.GetService(typeof(TelemetryClient));
                        return new ConnectorProcessor(connectorOptions, exchangeSelector, mediator, logger, telemetry);
                    });
            return services;
        }
    }
}