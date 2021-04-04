using MarketIntelligency.WebGrpc.Models;
using System.Threading.Tasks;

namespace MarketIntelligency.WebGrpc
{
    public interface IMarketIntelligencyGrpc
    {
        Task<Response> ActivateAsync(string exchangeName);
        Task<Response> DeactivateAsync(string exchangeName);
    }
}