using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.DataEventManager.ConnectorAggregate
{
    public partial class ConnectorProcessor : IHostedService
    {
        private readonly ConnectorOptions _options;
        private readonly ILogger<ConnectorProcessor> _logger;
        private readonly IExchangeSelector _exchangeSelector;
        private readonly IMediator _mediator;
        private readonly TelemetryClient _telemetryClient;
        public ConnectorProcessor(Action<ConnectorOptions> connectorOptions, IExchangeSelector exchangeSelector, IMediator mediator, ILogger<ConnectorProcessor> logger, TelemetryClient telemetryClient)
        {
            connectorOptions = connectorOptions ?? throw new ArgumentNullException(nameof(connectorOptions));
            var connectorOptionsModel = new ConnectorOptions();
            connectorOptions.Invoke(connectorOptionsModel);
            _options = connectorOptionsModel;
            _exchangeSelector = exchangeSelector ?? throw new ArgumentNullException(nameof(exchangeSelector));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (ExchangeName.IsValid(_options.Name))
            {
                var exchangeName = Enumeration.FromDisplayName<ExchangeName>(_options.Name);
                var exchange = _exchangeSelector.GetByName(exchangeName);
                var market = exchange.Info.Markets.First();
                await ConnectToRest(market, cancellationToken, _options.TimeFrame, (a, c) => exchange.FetchOrderBookAsync(a, c).Result);
                Console.WriteLine("Exchange Connector Service Activated");
            }
            else
            {
                // TODO: Section reserved for non exchange connectors activation;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}