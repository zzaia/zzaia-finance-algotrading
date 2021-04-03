using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;

namespace MarketIntelligency.Connector
{
    public partial class ConnectorProcessor
    {
        private static class Log
        {
            public static class CallToRest
            {
                public static void Received(ILogger logger, string timestamp)
                    => _receivedMessage(logger, timestamp, null);
                public static void WithFailedResponse(ILogger logger, string payLoad)
                    => _withFailedResponse(logger, payLoad, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);

                #region Logging Messages
                private static readonly Action<ILogger, string, Exception> _receivedMessage =
                    LoggerMessage.Define<string>(LogLevel.Information, new EventId(1101, "ConnectToRestReceived")
                    , "Call to connect to rest api received at {timestamp}");
                private static readonly Action<ILogger, string, Exception> _withFailedResponse =
                    LoggerMessage.Define<string>(LogLevel.Error, new EventId(1102, "ConnectToRestFailedResponse")
                    , "Was not possible to connect to a rest api due to a failed response without a description. Payload = {payLoad}");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(1103, "ConnectToRestWithException")
                    , "Was not possible to connect to a rest api due to an exception.");

                #endregion

                #region Logging Action Event
                public static void ReceivedAction(TelemetryClient telemetryClient)
                    => telemetryClient.TrackEvent("Connect:Rest");
                #endregion
            }
        }
    }
}