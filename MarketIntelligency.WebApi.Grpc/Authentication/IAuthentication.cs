using System.Threading.Tasks;

namespace MarketIntelligency.WebApi.Grpc.Authentication
{
    public interface IAuthentication
    {
        Task<OAuthToken> GetTokenAsync();
    }
}