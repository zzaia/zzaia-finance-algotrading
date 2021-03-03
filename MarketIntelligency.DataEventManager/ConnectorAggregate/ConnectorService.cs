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

namespace MarketIntelligency.DataEventManager.ConnectorAggregate
{
    public partial class ConnectorService
    {
        private readonly IExchangeSelector _exchangeSelector;
        private readonly ConnectorOptions _options;
        private readonly IMediator _mediator;
        protected ILogger<ConnectorService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ConnectorService(IExchangeSelector exchangeSelector, IMediator mediator, ILogger<ConnectorService> logger, TelemetryClient telemetryClient/*, IEnumerable<IOptionsMonitor<ConnectorOptions>> options*/)
        {
            //TODO: The configuration must be selected from a collection of options or the connector instance must be initialized in the startup;
            //_options = options.ToList().Any() ? options.Select() : throw new ArgumentNullException(nameof(options));
            _exchangeSelector = exchangeSelector ?? throw new ArgumentNullException(nameof(exchangeSelector));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public ConnectorService(ConnectorOptions connectorOptions)
        {
            _options = connectorOptions ?? throw new ArgumentNullException(nameof(connectorOptions));
        }

        public async void Activate()
        {
            Console.WriteLine("Connector Service Activated");
            //await ConnectToRest<Market, OrderBook>()
            //TODO: In this method the connect must be initialized by configurations, setting the time frame, the data type in and out;
        }

        public async void Deactivate()
        {
            Console.WriteLine("Connector Service Deactivated");
            //await ConnectToRest<Market, OrderBook>()
            //TODO: In this method the connect must be initialized by configurations, setting the time frame, the data type in and out;
        }

        private async Task ConnectToRest<T, TResult>(T parameter, CancellationToken cancellationToken, TimeFrame timeFrame, Func<T, CancellationToken, ObjectResult<TResult>> method) where TResult : class
        {
            Log.ConnectToRest.Received(_logger);
            Log.ConnectToRest.ReceivedAction(_telemetryClient);
            var timeOutCancellationTokenSource = new CancellationTokenSource();
            timeOutCancellationTokenSource.CancelAfter(timeFrame.TimeSpan.Milliseconds);
            var timeoutCancellationToken = timeOutCancellationTokenSource.Token;
            var initialTime = DateTimeOffset.UtcNow;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = method.Invoke(parameter, timeoutCancellationToken);
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
                    var finalTime = DateTimeOffset.UtcNow;
                    var awaitTime = (initialTime + timeFrame.TimeSpan) - finalTime;
                    await Task.Delay(awaitTime.Milliseconds);
                }
                catch (TimeoutException ex)
                {
                    // Task was canceled by timeout.
                    Log.ConnectToRest.WithException(_logger, ex);
                    timeOutCancellationTokenSource.Dispose();
                    timeOutCancellationTokenSource = new CancellationTokenSource();
                    timeOutCancellationTokenSource.CancelAfter(timeFrame.TimeSpan.Milliseconds);
                    // TODO : reset the cancelation token to be able to continue in the loop.
                }
                catch (TaskCanceledException ex)
                {
                    // Task was canceled before running.
                    Log.ConnectToRest.WithException(_logger, ex);
                    break;
                }
                catch (OperationCanceledException ex)
                {
                    // Task was canceled while running.
                    Log.ConnectToRest.WithException(_logger, ex);
                    break;
                }
            }
        }
    }
}