using Zzaia.Finance.Core.Interfaces.ExchangeAggregate;
using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Microsoft.Extensions.Logging;
using System;

namespace Zzaia.Finance.Connector
{
    public partial class ExchangeSelector : IExchangeSelector
    {
        private readonly ILogger<ExchangeSelector> _logger;
        private readonly IServiceProvider _provider;

        public ExchangeSelector(ILogger<ExchangeSelector> logger, IServiceProvider provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
        }

        /// <summary>
        /// Select exchange web api by name using reflection, uppon the runtime.
        /// </summary>
        public IExchange SelectByName(ExchangeName exchangeName) { return SelectExchangeWebApi(exchangeName); }

        private IExchange SelectExchangeWebApi(ExchangeName exchangeName)
        {
            Log.SelectExchangetWebApi.Received(_logger);
            if (exchangeName is null)
            {
                Log.SelectExchangetWebApi.WithBadRequest(_logger);
                return default;
            }

            Type type;
            if (exchangeName.Equals(ExchangeName.MercadoBitcoin))
            {
                type = typeof(Exchange.MercadoBitcoin.MercadoBitcoinExchange);
            }
            else if (exchangeName.Equals(ExchangeName.Binance))
            {
                type = typeof(Exchange.Binance.BinanceExchange);
            }
            else if (exchangeName.Equals(ExchangeName.Ftx))
            {
                type = typeof(Exchange.Ftx.FtxExchange);
            }
            else
            {
                Log.SelectExchangetWebApi.WithInvalidNamespace(_logger);
                return default;
            }

            IExchange service = (IExchange)_provider.GetService(type);
            if (service == null)
            {
                Log.SelectExchangetWebApi.CouldNotResolveService(_logger);
                return default;
            }
            return service;
        }
    }
}