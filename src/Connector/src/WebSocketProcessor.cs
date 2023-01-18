using Zzaia.Finance.Core.Interfaces.ExchangeAggregate;
using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.MarketAgregate;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using Zzaia.Finance.EventManager;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zzaia.Finance.Connector
{
    public partial class WebSocketProcessor : BackgroundService
    {
        private readonly WebSocketConnectorOptions _options;
        private readonly IExchangeSelector _exchangeSelector;
        private readonly IDataStreamSource _dataStreamSource;
        private readonly ILogger<WebSocketProcessor> _logger;
        private readonly TelemetryClient _telemetryClient;
        private DateTimeOffset _lastStartTime;

        /// <summary>
        /// Provide data streams from web socket clients
        /// </summary>
        public WebSocketProcessor(Action<WebSocketConnectorOptions> connectorOptions,
            IExchangeSelector exchangeSelector,
            IDataStreamSource dataStreamSource,
            ILogger<WebSocketProcessor> logger,
            TelemetryClient telemetryClient)
        {
            connectorOptions = connectorOptions ?? throw new ArgumentNullException(nameof(connectorOptions));
            var connectorOptionsModel = new WebSocketConnectorOptions();
            connectorOptions.Invoke(connectorOptionsModel);
            _options = connectorOptionsModel;
            _exchangeSelector = exchangeSelector ?? throw new ArgumentNullException(nameof(exchangeSelector));
            _dataStreamSource = dataStreamSource ?? throw new ArgumentNullException(nameof(dataStreamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.DataIn.Any()) new ArgumentException("No subscriptions data in type provided", nameof(_options.DataIn));
            if (!_options.DataOut.Any()) new ArgumentException("No subscriptions data out type provided", nameof(_options.DataOut));
            var exchange = _exchangeSelector.SelectByName(_options.ExchangeName);
            if (exchange.Info.Options.HasWebSocket)
            {
                await exchange.InitializeAsync(stoppingToken);
                foreach (var item in _options.DataIn)
                {
                    if (item.GetType() == typeof(Market))
                    {
                        if (_options.DataOut.Any(each => each.Name.Equals(nameof(OrderBook))))
                        {
                            await exchange.SubscribeOrderbookAsync(item, stoppingToken);
                        }
                    }
                }
                _lastStartTime = DateTimeOffset.UtcNow;
                Log.CalltoWebsocket.Received(_logger);
                Log.CalltoWebsocket.ReceivedAction(_telemetryClient);
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var utcNow = DateTimeOffset.UtcNow;
                        if (utcNow - _lastStartTime > exchange.Info.Options.CheckForLivenessTimeSpan)
                        {
                            await exchange.ConfirmLivenessAsync(stoppingToken);
                            _lastStartTime = utcNow;
                        }

                        await exchange.ReceiveAsync(PublishEvent, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Log.CalltoWebsocket.WithException(_logger, ex);
                        await exchange.RestartAsync(stoppingToken);
                    }
                }
            }
            else
            {
                Log.CalltoWebsocket.WithOutWebsocketSupport(_logger);
            }
        }

        private void PublishEvent<T>(T content) where T : class
        {
            _dataStreamSource.Publish(new EventSource<T>(content));
        }
    }
}
