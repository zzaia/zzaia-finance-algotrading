using System.Net.Http;

namespace MarketIntelligency.WebApi.Grpc.Authentication
{
    public class GrpcAuthentication : Authentication, IGrpcAuthentication
    {
        public GrpcAuthentication(HttpClient httpClient, string clientId, string clientSecret, string tokenIssuer, string scope) : base(httpClient: httpClient, clientId: clientId, clientSecret: clientSecret, tokenIssuer: tokenIssuer, scope: scope)
        {
        }
    }
}