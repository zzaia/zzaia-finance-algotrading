using Grpc.Core;
using MarketIntelligency.DataEventManager.ConnectorAggregate;
using MarketIntelligency.WebApi.Grpc.Protos;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.WebApi.Services
{
    public partial class ControlService : ControlGrpc.ControlGrpcBase
    {
        protected ILogger<ControlService> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly IServiceProvider _serviceProvider;
        public ControlService(ILogger<ControlService> logger, TelemetryClient telemetryClient, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }
        public override async Task<Google.Protobuf.WellKnownTypes.Empty> Activate(ControlMetadata request, ServerCallContext context)
        {
            Log.Activate.Received(_logger);
            Log.Activate.ReceivedAction(_telemetryClient);

            try
            {
                if (request.Name.Equals("connectors"))
                {
                    var services = _serviceProvider.GetServices<IConnectorControl>();
                    foreach (var service in services)
                    {
                        service.Activate();
                    }
                    context.Status = Status.DefaultSuccess;
                    return null;
                }
                else
                {
                    Log.Activate.WithBadRequest(_logger, request.Name);
                    context.Status = new Status(StatusCode.InvalidArgument, "Invalid Argument Name");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Activate.WithException(_logger, ex);
                context.Status = new Status(StatusCode.Internal, ex.Message);
                return null;
            }
        }

        public override Task<Google.Protobuf.WellKnownTypes.Empty> Deactivate(ControlMetadata request, ServerCallContext context)
        {
            return base.Deactivate(request, context);
        }
    }
}