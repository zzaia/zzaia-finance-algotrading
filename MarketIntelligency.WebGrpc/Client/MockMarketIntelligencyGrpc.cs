using MarketIntelligency.WebGrpc.Models;
using MarketIntelligency.WebGrpc.Protos;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.WebGrpc
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