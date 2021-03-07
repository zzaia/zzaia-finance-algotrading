using MarketIntelligency.WebApi.Grpc.Models;
using MarketIntelligency.WebApi.Grpc.Protos;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.WebApi.Grpc
{
    public class MockMarketIntelligencyGrpc : IMarketIntelligencyGrpc
    {
        public Task<Response> ActivateAsync(string exchangeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeactivateAsync(string exchangeName)
        {
            throw new NotImplementedException();
        }
    }
}