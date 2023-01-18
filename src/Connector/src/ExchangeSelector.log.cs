using Microsoft.Extensions.Logging;
using System;

namespace Zzaia.Finance.Connector
{
    public partial class ExchangeSelector
    {
        private static class Log
        {
            public static class SelectExchangetWebApi
            {
                public static void Received(ILogger logger)
                    => _receivedMessage(logger, null);
                public static void WithBadRequest(ILogger logger)
                    => _withBadRequest(logger, null);
                public static void WithInvalidNamespace(ILogger logger)
                    => _withInvalidNamespace(logger, null);
                public static void CouldNotResolveService(ILogger logger)
                    => _couldNotResolveService(logger, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);

                #region Logging Messages
                private static readonly Action<ILogger, Exception> _receivedMessage =
                    LoggerMessage.Define(LogLevel.Information, new EventId(101, "SelectExchangeReceived")
                    , "Call to select the exhange web api received");
                private static readonly Action<ILogger, Exception> _withBadRequest =
                    LoggerMessage.Define(LogLevel.Error, new EventId(102, "SelectExchangeWithNullName")
                    , "Was not possible to select the exchange web api due to an null exchange name.");
                private static readonly Action<ILogger, Exception> _withInvalidNamespace =
                    LoggerMessage.Define(LogLevel.Error, new EventId(103, "SelectExchangeWithInvalidNamespace")
                    , "Was not possible to retrieve the exchange type due to a invalid namespace.");
                private static readonly Action<ILogger, Exception> _couldNotResolveService =
                    LoggerMessage.Define(LogLevel.Error, new EventId(104, "ExchangeServiceCouldNotResolved")
                    , "Was not possible to resolve the registered exchange Service. Make sure it is in IoC.");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(105, "SelectExchangeWithException")
                    , "Was not possible to select the exchange web api due to an exception.");

                #endregion

            }
        }
    }
}
