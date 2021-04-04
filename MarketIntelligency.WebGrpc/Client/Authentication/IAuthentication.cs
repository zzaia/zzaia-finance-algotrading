using System.Threading.Tasks;

namespace MarketIntelligency.WebGrpc.Authentication
{
    public interface IAuthentication
    {
        Task<OAuthToken> GetTokenAsync();
    }
}