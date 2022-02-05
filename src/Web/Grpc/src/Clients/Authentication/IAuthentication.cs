using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Clients.Authentication
{
    public interface IAuthentication
    {
        Task<OAuthToken> GetTokenAsync();
    }
}