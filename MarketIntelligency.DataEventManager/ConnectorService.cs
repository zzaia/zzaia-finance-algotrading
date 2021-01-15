using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.DataEventManager
{
    public partial class ConnectorService
    {
        private readonly IExchange _exchange;
        private readonly IMediator _mediator;
        protected ILogger<ConnectorService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ConnectorService(IExchange exchange, IMediator mediator, ILogger<ConnectorService> logger, TelemetryClient telemetryClient)
        {
            _exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        private async Task ConnectToRest<T, TResult>(T parameter, CancellationToken cancellationToken, TimeFrame timeFrame, Func<T, CancellationToken, ObjectResult<TResult>> method) where TResult : class
        {
            Log.ConnectToRest.Received(_logger);
            Log.ConnectToRest.ReceivedAction(_telemetryClient);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var response = method.Invoke(parameter, cancellationToken);
                    if (response.Succeed)
                    {
                        var eventToPublish = new EventSource<TResult>(response.Output);
                        await _mediator.Publish(eventToPublish);
                    }
                    else
                    {
                        var errorPayload = JsonSerializer.Serialize(response.Error);
                        Log.ConnectToRest.WithFailedResponse(_logger, errorPayload);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ConnectToRest.WithException(_logger, ex);
            }
        }
    }
}