using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;

namespace Zzaia.Finance.Web.Grpc.Services
{
    public partial class EventService
    {
        public static class Log
        {
            public static class Run
            {
                public static void Received(ILogger logger)
                    => _initilizedMessage(logger, null);
                public static void WithBadRequest(ILogger logger, string payLoad)
                    => _withBadRequest(logger, payLoad, null);
                public static void WithException(ILogger logger, Exception exception)
                    => _withException(logger, exception);

                #region Logging Messages
                private static readonly Action<ILogger, Exception> _initilizedMessage =
                    LoggerMessage.Define(LogLevel.Information, new EventId(3101, "RunEvent")
                    , "Call to run event method");
                private static readonly Action<ILogger, string, Exception> _withBadRequest =
                    LoggerMessage.Define<string>(LogLevel.Error, new EventId(3102, "RunEventWithBadRequest")
                    , "Was not possible to run event due to bad request. Argument = {payLoad}");
                private static readonly Action<ILogger, Exception> _withException =
                    LoggerMessage.Define(LogLevel.Critical, new EventId(3103, "RunEventWithException")
                    , "Was not possible to run event due to an exception.");

                #endregion

                #region Logging Action Event
                public static void ReceivedAction(TelemetryClient telemetryClient)
                    => telemetryClient.TrackEvent("WebGrpc:RunEvent");
                #endregion
            }
        }
    }
}
