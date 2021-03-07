using Grpc.Core;
using MarketIntelligency.WebApi.Grpc.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.WebApi.Grpc
{
    public class GrpcBase
    {
        protected ILogger _logger;
        public async Task<Response<TResponse>> GetResponseAsync<TResponse>(AsyncUnaryCall<TResponse> response)
            where TResponse : class
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            var result = await response.ResponseAsync;
            var status = response.GetStatus();
            if (status.StatusCode == StatusCode.OK)
            {
                return new Response<TResponse>(result);
            }
            else
            {
                Log.WithError(_logger, status.StatusCode.ToString(), status.Detail);
                return new Response<TResponse>(status);
            }
        }
        public async Task<Response> GetResponseAsync(AsyncUnaryCall<Google.Protobuf.WellKnownTypes.Empty> response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            await response.ResponseAsync;
            var status = response.GetStatus();
            if (status.StatusCode == StatusCode.OK)
            {
                return new Response(true);
            }
            else
            {
                Log.WithError(_logger, status.StatusCode.ToString(), status.Detail);
                return new Response(status);
            }
        }
        private static class Log
        {
            public static void WithError(ILogger logger, string statusCode, string details)
                => _withError(logger, statusCode, details, null);

            private static readonly Action<ILogger, string, string, Exception> _withError =
                LoggerMessage.Define<string, string>(LogLevel.Critical, new EventId(1102, "GrpcResponseWithError")
                , "Client Grpc response returned {statusCode} {detail}");
        }
    }
}