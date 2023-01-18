using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;

namespace Zzaia.Finance.Connector
{
    public partial class WebSocketProcessor
    {

        private static class Log
        {
            public static class CalltoWebsocket
            {
                public static void Received(ILogger logger)
                    => _receivedMessage(logger, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);
                public static void WithOutWebsocketSupport(ILogger logger)
                    => _withoutsupport(logger, null);

                #region Logging Messages
                private static readonly Action<ILogger, Exception> _receivedMessage =
                    LoggerMessage.Define(LogLevel.Information, new EventId(3101, "ConnectToWebSocketReceived")
                    , "Call to connect to websocket received");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(3102, "ConnectToWebSocketWithException")
                    , "Was not possible to connect to a websocket due to an exception.");
                private static readonly Action<ILogger, Exception> _withoutsupport =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(3103, "WithOutSupportWebsocket")
                    , "Exchange does not support web socket.");

                #endregion

                #region Logging Action Event
                public static void ReceivedAction(TelemetryClient telemetryClient)
                    => telemetryClient.TrackEvent("Connect:WebSocket");
                #endregion
            }
        }
    }
}
