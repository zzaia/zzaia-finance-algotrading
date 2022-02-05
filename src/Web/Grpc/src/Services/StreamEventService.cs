using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Web.Grpc.Protos;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Services
{
    public partial class StreamEventService : StreamEventGrpc.StreamEventGrpcBase
    {
        private readonly StreamEventGrpc.StreamEventGrpcClient _client;

        public StreamEventService(StreamEventGrpc.StreamEventGrpcClient client)
        {
            _client = client;
        }


        public override async Task<Empty> ReceiveEvent(IAsyncStreamReader<EventMessage> responseStream, ServerCallContext context)
        {
            // Forward the call on to the greeter service
            using var call = _client.SendEvent(new Empty());
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                var unpackedData = response.Content.Unpack<Point>();
                var eventSource = new EventSource(response.Content,);
                //TODO: Publish to stream
            }
        }
    }
}
