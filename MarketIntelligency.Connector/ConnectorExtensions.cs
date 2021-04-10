using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.EventManager;
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
        /// Adds services and options for the web api connector.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="connectorOptions">A delegate to configure the connector opations<see cref="WebApiConnectorOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiConnector(this IServiceCollection services, Action<WebApiConnectorOptions> connectorOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddHostedService((s) =>
            {
                var source = (IDataStreamSource)s.GetService(typeof(IDataStreamSource));
                var exchangeSelector = (IExchangeSelector)s.GetService(typeof(IExchangeSelector));
                var logger = (ILogger<WebApiProcessor>)s.GetService(typeof(ILogger<WebApiProcessor>));
                var telemetry = (TelemetryClient)s.GetService(typeof(TelemetryClient));
                return new WebApiProcessor(connectorOptions, exchangeSelector, source, logger, telemetry);
            });
            return services;
        }

        /// <summary>
        /// Adds services and options for the web socket connector.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="connectorOptions">A delegate to configure the connector opations<see cref="WebSocketConnectorOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddWebSocketConnector(this IServiceCollection services, Action<WebSocketConnectorOptions> connectorOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddHostedService((s) =>
            {
                var source = (IDataStreamSource)s.GetService(typeof(IDataStreamSource));
                var exchangeSelector = (IExchangeSelector)s.GetService(typeof(IExchangeSelector));
                var logger = (ILogger<WebSocketProcessor>)s.GetService(typeof(ILogger<WebSocketProcessor>));
                var telemetry = (TelemetryClient)s.GetService(typeof(TelemetryClient));
                return new WebSocketProcessor(connectorOptions, exchangeSelector, source, logger, telemetry);
            });
            return services;
        }
    }
}