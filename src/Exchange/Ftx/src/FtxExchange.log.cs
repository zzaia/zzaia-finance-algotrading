using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;

namespace MarketIntelligency.Exchange.Ftx
{
    public partial class FtxExchange
    {
        private static class Log
        {
            public static class Websocket
            {
                public static void Received(ILogger logger)
                    => _receivedMessage(logger, null);
                public static void WithFailedResponse(ILogger logger, string code, string message)
                    => _withFailedResponse(logger, code, message, null);
                public static void WithOperationCanceled(ILogger logger)
                    => _withOperationCanceled(logger, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);

                #region Logging Messages
                private static readonly Action<ILogger, Exception> _receivedMessage =
                    LoggerMessage.Define(LogLevel.Information, new EventId(20101, "WebsocketCallReceived")
                    , "Websocket call received");
                private static readonly Action<ILogger, string, string, Exception> _withFailedResponse =
                    LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(202102, "WebsocketCallReceivedWithError")
                    , "Websocket call received came with a error response.Code = {code} Message = {message}");
                private static readonly Action<ILogger, Exception> _withOperationCanceled =
                    LoggerMessage.Define(LogLevel.Error, new EventId(20103, "WebsocketCallReceivedCanceled")
                    , "Was not possible to receive a websocket call due to an operation cancelation.");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(20104, "WebsocketCallReceivedWithException")
                    , "Was not possible to receive websocket due to an exception.");

                #endregion

                #region Logging Action Event
                public static void ReceivedAction(TelemetryClient telemetryClient)
                    => telemetryClient.TrackEvent("Ftx:Receive");
                #endregion
            }
        }
    }
}
