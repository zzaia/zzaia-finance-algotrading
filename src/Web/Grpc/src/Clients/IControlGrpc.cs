using MarketIntelligency.Web.Grpc.Models;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Clients
{
    public interface IControlGrpc
    {
        Task<Response> ActivateAsync(string exchangeName);
        Task<Response> DeactivateAsync(string exchangeName);
    }
}