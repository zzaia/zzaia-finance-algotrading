using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;

namespace MarketIntelligency.Web.Grpc.Services
{
    public partial class StreamEventService
    {
        private static class Log
        {
            public static class RunStream
            {
                public static void Received(ILogger logger)
                    => _initilizedMessage(logger, null);
                public static void WithBadRequest(ILogger logger, string payLoad)
                    => _withBadRequest(logger, payLoad, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);

                #region Logging Messages
                private static readonly Action<ILogger, Exception> _initilizedMessage =
                    LoggerMessage.Define(LogLevel.Information, new EventId(4101, "RunStreamInitialized")
                    , "Call to initiliaze stream");
                private static readonly Action<ILogger, string, Exception> _withBadRequest =
                    LoggerMessage.Define<string>(LogLevel.Error, new EventId(4102, "RunStreamWithBadRequest")
                    , "Was not possible to run stream due to bad request. Argument = {payLoad}");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(4103, "RunStreamWithException")
                    , "Was not possible to run stream due to an exception.");

                #endregion

                #region Logging Action Event
                public static void ReceivedAction(TelemetryClient telemetryClient)
                    => telemetryClient.TrackEvent("WebGrpc:RunStream");
                #endregion
            }
        }
    }
}