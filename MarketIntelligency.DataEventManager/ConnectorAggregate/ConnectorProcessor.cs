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
        //private delegate Task MethodHandler<T, TResult>(Func<T, CancellationToken, ObjectResult<TResult>> method) where TResult : class;
        private delegate ObjectResult<TResult> MethodHandler<T, CancellationToken, TResult>(T arg, CancellationToken cancellationToken) where TResult : class;
        private List<Tuple<Market, MethodHandler<Market, CancellationToken, OrderBook>>> _delegatesCollection { get; set; }


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

        //private async Task CallToRest<T, TResult>(T parameter, CancellationToken cancellationToken, TimeFrame timeFrame, MethodHandler<T, CancellationToken, TResult> method) where TResult : class
        //{
        //    Log.CallToRest.Received(_logger);
        //    Log.CallToRest.ReceivedAction(_telemetryClient);

        //    try
        //    {
        //        var result = method.Invoke(parameter, timeoutCancellationToken);
        //        if (result.Succeed)
        //        {
        //            var eventToPublish = new EventSource<TResult>(result.Output);
        //            await _mediator.Publish(eventToPublish);
        //        }
        //        else
        //        {
        //            var errorMessage = $"{result.Error.Status} - {result.Error.Detail}";
        //            Log.CallToRest.WithFailedResponse(_logger, errorMessage);
        //        }
        //        var finalTime = DateTimeUtils.CurrentUtcTimestamp();
        //        var awaitTime = (initialTime + timeFrame.TimeSpan) - finalTime;
        //        await Task.Delay(awaitTime, cancellationToken);
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        // Task was canceled by timeout.
        //        Log.CallToRest.WithException(_logger, ex);
        //        timeOutCancellationTokenSource.Dispose();
        //        timeOutCancellationTokenSource = new CancellationTokenSource();
        //        timeOutCancellationTokenSource.CancelAfter(timeFrame.TimeSpan);
        //        // TODO : reset the cancelation token to be able to continue in the loop.
        //    }
        //    catch (TaskCanceledException ex)
        //    {
        //        // Task was canceled before running.
        //        Log.CallToRest.WithException(_logger, ex);
        //    }
        //    catch (OperationCanceledException ex)
        //    {
        //        // Task was canceled while running.
        //        Log.CallToRest.WithException(_logger, ex);
        //    }
        //}

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (ExchangeName.IsValid(_options.Name))
            {
                var exchangeName = Enumeration.FromDisplayName<ExchangeName>(_options.Name);
                var exchange = _exchangeSelector.GetByName(exchangeName);
                _delegatesCollection = new List<Tuple<Market, MethodHandler<Market, CancellationToken, OrderBook>>>();
                foreach (var market in exchange.Info.Markets)
                {
                    //var del = new Func<Market, CancellationToken, ObjectResult<OrderBook>>((a, c) => exchange.FetchOrderBookAsync(a, c).Result);
                    var del = new MethodHandler<Market, CancellationToken, OrderBook>((a, c) => exchange.FetchOrderBookAsync(a, c).Result);
                    var tuple = new Tuple<Market, MethodHandler<Market, CancellationToken, OrderBook>>(market, del);
                    _delegatesCollection.Add(tuple);
                }

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var timeNow = DateTimeUtils.CurrentUtcTimestamp();
                        var timeFrame = _options.TimeFrame.TimeSpan;
                        var timeCount = timeNow % timeFrame.TotalMilliseconds;
                        var period = timeFrame / 2000;
                        while (!cancellationToken.IsCancellationRequested && timeCount > 2 * period.Milliseconds)
                        {
                            timeNow = DateTimeUtils.CurrentUtcTimestamp();
                            timeCount = timeNow % timeFrame.TotalMilliseconds;
                            await Task.Delay(period, cancellationToken);
                        }

                        Parallel.ForEach(_delegatesCollection, async (item) =>
                        {
                            Log.CallToRest.Received(_logger);
                            Log.CallToRest.ReceivedAction(_telemetryClient);
                            var result = item.Item2.Invoke(item.Item1, cancellationToken);
                            if (result.Succeed)
                            {
                                await PublishEvent(result.Output);
                            }
                            else
                            {
                                var errorMessage = $"{result.Error.Status} - {result.Error.Detail}";
                                Log.CallToRest.WithFailedResponse(_logger, errorMessage);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.CallToRest.WithException(_logger, ex);
                    }
                }
            }
            else
            {
                // TODO: Section reserved for non exchange connectors activation;
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