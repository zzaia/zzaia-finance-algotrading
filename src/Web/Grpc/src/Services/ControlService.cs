using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Zzaia.Finance.Web.Grpc.Protos;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Zzaia.Finance.Web.Grpc.Services
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

        public override async Task<Empty> Activate(ControlMetadataDTO request, ServerCallContext context)
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
                    return await Task.FromResult(new Empty());
                }
                else
                {
                    Log.Activate.WithBadRequest(_logger, request.Name);
                    context.Status = new Status(StatusCode.InvalidArgument, "Invalid Argument Name");
                    return await Task.FromResult(new Empty());
                }
            }
            catch (Exception ex)
            {
                Log.Activate.WithException(_logger, ex);
                context.Status = new Status(StatusCode.Internal, ex.Message);
                return await Task.FromResult(new Empty());
            }
        }

        public override Task<Empty> Deactivate(ControlMetadataDTO request, ServerCallContext context)
        {
            return base.Deactivate(request, context);
        }
    }
}