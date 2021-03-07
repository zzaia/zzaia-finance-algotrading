﻿using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;

namespace MarketIntelligency.Exchange.MercadoBitcoin
{
    public partial class MercadoBitcoinExchange
    {
        private static class Log
        {
            public static class FetchOrderBook
            {
                public static void Received(ILogger logger)
                    => _receivedMessage(logger, null);
                public static void WithFailedResponse(ILogger logger, string payLoad)
                    => _withFailedResponse(logger, payLoad, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);

                #region Logging Messages
                private static readonly Action<ILogger, Exception> _receivedMessage =
                    LoggerMessage.Define(LogLevel.Information, new EventId(10101, "FetchOrderBookReceived")
                    , "Call to fetch orderbook received");
                private static readonly Action<ILogger, string, Exception> _withFailedResponse =
                    LoggerMessage.Define<string>(LogLevel.Error, new EventId(102102, "FetchOrderBookWithFailedResponse")
                    , "Was not possible to fetch orderbook due to a failed response without a description. Payload = {payLoad}");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(10103, "FetchOrderBookWithException")
                    , "Was not possible to fetch orderbook due to an exception.");

                #endregion

                #region Logging Action Event
                public static void ReceivedAction(TelemetryClient telemetryClient)
                    => telemetryClient.TrackEvent("MercadoBitcoin:FetchOrderBook");
                #endregion
            }
        }
    }
}