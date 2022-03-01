using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.Core.Utils;
using MarketIntelligency.EventManager;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Connector
{
    public partial class WebApiProcessor : BackgroundService
    {
        private readonly WebApiConnectorOptions _options;
        private readonly IExchangeSelector _exchangeSelector;
        private readonly IDataStreamSource _dataStreamSource;
        private readonly ILogger<WebApiProcessor> _logger;
        private readonly TelemetryClient _telemetryClient;
        private delegate ObjectResult<TResult> MethodHandler<T, CancellationToken, TResult>(T arg, CancellationToken cancellationToken) where TResult : class;
        private List<(dynamic, Delegate)> _delegateCollection { get; set; }

        /// <summary>
        /// Provide data streams from web api clients
        /// </summary>
        public WebApiProcessor(Action<WebApiConnectorOptions> connectorOptions,
            IExchangeSelector exchangeSelector,
            IDataStreamSource dataStreamSource,
            ILogger<WebApiProcessor> logger,
            TelemetryClient telemetryClient)
        {
            connectorOptions = connectorOptions ?? throw new ArgumentNullException(nameof(connectorOptions));
            var connectorOptionsModel = new WebApiConnectorOptions();
            connectorOptions.Invoke(connectorOptionsModel);
            _options = connectorOptionsModel;
            _exchangeSelector = exchangeSelector ?? throw new ArgumentNullException(nameof(exchangeSelector));
            _dataStreamSource = dataStreamSource ?? throw new ArgumentNullException(nameof(dataStreamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            TimeSpan minTimeFrame = new();
            var exchange = _exchangeSelector.SelectByName(_options.ExchangeName);
            if (!exchange.Info.Options.HasWebApi)
            {
                throw new ArgumentException("Exchange does not support web api.", nameof(ExchangeName));
            }

            minTimeFrame = exchange.Info.LimitRate.Period;
            _delegateCollection = new List<(dynamic, Delegate)>();
            foreach (var item in _options.DataIn)
            {
                if (item.GetType() == typeof(Market))
                {
                    if (_options.DataOut.Any(each => each.Equals(typeof(OrderBook))))
                    {
                        var del = new MethodHandler<Market, CancellationToken, OrderBook>((a, c) => exchange.FetchOrderBookAsync(a, c).Result);
                        _delegateCollection.Add((item, del));
                    }
                }
            }

            while (!cancellationToken.IsCancellationRequested && _delegateCollection.Any())
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
                    CancellationToken = cancellationToken,
                    MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism
                };

                var parallelLoop = Parallel.ForEach(_delegateCollection, paralletOptions, (item) =>
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
                            PublishEvent(result.Output);
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

        private void PublishEvent<T>(T content) where T : class
        {
            var eventToPublish = new EventSource<T>(content);
            _dataStreamSource.Publish(eventToPublish);
        }
    }
}