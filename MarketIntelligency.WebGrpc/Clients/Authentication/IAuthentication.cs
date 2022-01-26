using System.Threading.Tasks;

namespace MarketIntelligency.WebGrpc.Clients.Authentication
{
    public interface IAuthentication
    {
        Task<OAuthToken> GetTokenAsync();
    }
}