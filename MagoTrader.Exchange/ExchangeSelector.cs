using MagoTrader.Core.Exchange;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace MagoTrader.Exchange
{
    public class ExchangeSelector : IExchangeSelector
    {
        private readonly ILogger<ExchangeSelector> _logger;
        private readonly IServiceProvider _provider;
        private readonly string _notValidTypeMessage = "is not a valid exchange type.";
        private readonly string _notRegisteredServiceMessage = " can't be resolved. Make sure the exchange is registered with the IoC container in startup.cs.";

        public ExchangeSelector(ILogger<ExchangeSelector> logger, IServiceProvider provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
        }

        /// <summary>
        /// Get exchange by name and reflection, on the runtime.
        /// </summary>
        public IExchange GetByName(ExchangeNameEnum exchangeName) { return GetExchangeByNamespace<IExchange>(exchangeName); }

        private Ttype GetExchangeByNamespace<Ttype>(ExchangeNameEnum exchangeName)
        {

            Type type = Type.GetType(String.Format(CultureInfo.InvariantCulture,
                                                          "{0}.{1}.{1}Exchange",
                                                       this.GetType().Namespace,
                                                       exchangeName.ToString()));
            if (type == null)
            {
                var errorMessage = $"'{type}' {_notValidTypeMessage}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            Ttype service = (Ttype)_provider.GetService(type);
            if (service == null)
            {
                var errorMessage = $"'{service}' {_notRegisteredServiceMessage}";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            return service;
        }
    }
}
