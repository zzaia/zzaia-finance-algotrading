using MarketIntelligency.WebApi.Grpc.Models;
using System.Threading.Tasks;

namespace MarketIntelligency.WebApi.Grpc
{
    public interface IMarketIntelligencyGrpc
    {
        Task<Response> ActivateAsync(string exchangeName);
        Task<Response> DeactivateAsync(string exchangeName);
    }
}