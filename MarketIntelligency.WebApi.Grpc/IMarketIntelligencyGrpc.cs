using MarketIntelligency.WebApi.Grpc.Models;
using MarketIntelligency.WebApi.Grpc.Protos;
using System.Threading.Tasks;

namespace MarketIntelligency.WebApi.Grpc
{
    public interface IMarketIntelligencyGrpc
    {
        Task<Response<ControlResponse>> ActivateAsync(string exchangeName);
        Task<Response<ControlResponse>> DeactivateAsync(string exchangeName);
    }
}