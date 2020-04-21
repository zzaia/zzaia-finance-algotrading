using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;

namespace MagoTrader.Exchange
{
    public class ExchangeSelector : IExchangeSelector
    {
        protected readonly ILogger<ExchangeSelector> _logger;
        protected readonly IServiceProvider _provider;

        public ExchangeSelector(ILogger<ExchangeSelector> logger, IServiceProvider provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
        }

        public IExchange GetByName(ExchangeNameEnum exchangeName) { return GetExchangeByNamespace<IExchange>(exchangeName); }

        private Ttype GetExchangeByNamespace<Ttype>(ExchangeNameEnum exchangeName)
        {

            Type type = Type.GetType(
            String.Format("{0}.{1}.{1}",
            this.GetType().Namespace,
            exchangeName.ToString()));
            if (type == null) { 
                var errorMessage = $"'{type}' is not a valid exchange type.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            Ttype service = (Ttype)_provider.GetService(type);
            if (service == null) { 
                var errorMessage = $"Can't resolve '{service.ToString()}'. Make sure the exchange is registered with the IoC container in startup.cs.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            return service;
        }
    }
}
