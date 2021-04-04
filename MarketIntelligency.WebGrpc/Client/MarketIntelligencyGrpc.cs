using Grpc.Core;
using MarketIntelligency.WebGrpc.Authentication;
using MarketIntelligency.WebGrpc.Models;
using MarketIntelligency.WebGrpc.Protos;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.WebGrpc
{
    public class MarketIntelligencyGrpc : GrpcBase, IMarketIntelligencyGrpc
    {
        private readonly ControlGrpc.ControlGrpcClient _controlGrpcClient;
        private readonly IGrpcAuthentication _authentication;

        public MarketIntelligencyGrpc(
            ControlGrpc.ControlGrpcClient controlGgrpcClient,
            IGrpcAuthentication authentication)
        {
            _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
            _controlGrpcClient = controlGgrpcClient ?? throw new ArgumentNullException(nameof(controlGgrpcClient));
        }

        public async Task<Response> ActivateAsync(string exchangeName)
        {
            OAuthToken oauthToken = await _authentication.GetTokenAsync();
            string accessToken = oauthToken.access_token;
            var headers = new Metadata
            {
                { "Authorization", $"Bearer {accessToken}" }
            };
            var metadataToRequest = new ControlMetadata()
            {

            };
            var response = _controlGrpcClient.ActivateAsync(metadataToRequest, headers);
            return await GetResponseAsync(response);
        }

        public async Task<Response> DeactivateAsync(string exchangeName)
        {
            OAuthToken oauthToken = await _authentication.GetTokenAsync();
            string accessToken = oauthToken.access_token;
            var headers = new Metadata
            {
                { "Authorization", $"Bearer {accessToken}" }
            };
            var metadataToRequest = new ControlMetadata()
            {

            };
            var response = _controlGrpcClient.DeactivateAsync(metadataToRequest, headers);
            return await GetResponseAsync(response);
        }
    }
}