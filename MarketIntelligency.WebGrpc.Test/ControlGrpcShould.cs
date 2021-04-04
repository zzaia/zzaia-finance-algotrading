using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Net.Client;
using MarketIntelligency.WebGrpc.Protos;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarketIntelligency.WebGrpc.Test
{
    public class ControlGrpcShould
    {
        [Fact]
        public void ActivateAGivenExchange()
        {
            // Arrange
            var mockClient = new Mock<ControlGrpc.ControlGrpcClient>();
            var controlArgs = new ControlMetadata() { Name = "connectors" };
            // Use a factory method provided by Grpc.Core.Testing.TestCalls to create an instance of a call.
            var fakeCall = TestCalls.AsyncUnaryCall(Task.FromResult(new Empty()), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { });

            // Act
            mockClient.Setup(m => m.ActivateAsync(It.IsAny<ControlMetadata>()
                , null, null, CancellationToken.None)).Returns(fakeCall);

            // Assert
            Assert.Equal(fakeCall, mockClient.Object.ActivateAsync(controlArgs));
        }

        [Fact]
        public async Task ActivateAllConnectors()
        {
            var options = new GrpcChannelOptions();
            var channel = GrpcChannel.ForAddress("https://localhost:5001", options);
            var client = new ControlGrpc.ControlGrpcClient(channel);

            // Asynchronous unary RPC
            var foosRequest = new ControlMetadata { Name = "connectors" };
            var fooResponse = await client.ActivateAsync(foosRequest);
        }
    }
}