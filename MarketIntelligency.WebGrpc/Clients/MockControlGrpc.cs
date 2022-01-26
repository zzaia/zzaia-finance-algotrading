using MarketIntelligency.WebGrpc.Models;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.WebGrpc.Clients
{
    public class MockControlGrpc : IControlGrpc
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