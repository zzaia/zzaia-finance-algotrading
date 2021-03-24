using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Utils;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private delegate ObjectResult<TResult> MethodHandler<T, CancellationToken, TResult>(T arg, CancellationToken cancellationToken) where TResult : class;
        private List<(dynamic, Delegate)> _delegateCollection { get; set; }


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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            TimeSpan minTimeFrame = new();
            if (ExchangeName.IsValid(_options.Name))
            {
                var exchangeName = Enumeration.FromDisplayName<ExchangeName>(_options.Name);
                var exchange = _exchangeSelector.GetByName(exchangeName);
                minTimeFrame = exchange.Info.LimitRate.Period;
                _delegateCollection = new List<(dynamic, Delegate)>();
                foreach (var market in _options.DataIn)
                {
                    if (_options.DataOut.Any(each => each.Equals(typeof(OrderBook))))
                    {
                        var del = new MethodHandler<Market, CancellationToken, OrderBook>((a, c) => exchange.FetchOrderBookAsync(a, c).Result);
                        _delegateCollection.Add((market, del));
                    }
                }
            }
            else
            {
                // TODO: Section reserved for non exchange connectors activation;
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                var timeNow = DateTimeUtils.CurrentUtcTimestamp();
                var timeFrame = _options.TimeFrame.TimeSpan > minTimeFrame ? _options.TimeFrame.TimeSpan : minTimeFrame;
                var timeCount = timeNow % timeFrame.TotalMilliseconds;
                var period = timeFrame / _options.Resolution;
                while (timeCount > _options.Tolerance * period.Milliseconds)
                {
                    timeNow = DateTimeUtils.CurrentUtcTimestamp();
                    timeCount = timeNow % timeFrame.TotalMilliseconds;
                    await Task.Delay(period, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                var paralletOptions = new ParallelOptions()
                {
                    CancellationToken = cancellationToken
                };

                var parallelLoop = Parallel.ForEach(_delegateCollection, paralletOptions, async (item) =>
                {
                    Log.CallToRest.Received(_logger);
                    Log.CallToRest.ReceivedAction(_telemetryClient);
                    try
                    {
                        var timeOutCancellationTokenSource = new CancellationTokenSource();
                        var timeOutCancellationToken = timeOutCancellationTokenSource.Token;
                        timeOutCancellationTokenSource.CancelAfter(Convert.ToInt32(timeFrame.TotalMilliseconds));
                        var result = item.Item2.DynamicInvoke(item.Item1, timeOutCancellationToken);
                        if (result.Succeed)
                        {
                            await PublishEvent(result.Output);
                        }
                        else
                        {
                            var errorMessage = $"{result.Error.Status} - {result.Error.Detail}";
                            Log.CallToRest.WithFailedResponse(_logger, errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.CallToRest.WithException(_logger, ex);
                    }
                });
            }
        }

        private async Task PublishEvent<T>(T content) where T : class
        {
            var eventToPublish = new EventSource<T>(content);
            await _mediator.Publish(eventToPublish);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}