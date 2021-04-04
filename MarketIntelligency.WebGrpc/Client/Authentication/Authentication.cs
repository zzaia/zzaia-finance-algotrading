using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MarketIntelligency.WebGrpc.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly string _tokenIssuer;
        private readonly List<KeyValuePair<string, string>> _parameters;
        private readonly HttpClient _httpClient;
        private OAuthToken _oauthToken;

        public Authentication(HttpClient httpClient, string clientId, string clientSecret, string tokenIssuer, string scope)
        {
            string _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            string _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            string _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            _tokenIssuer = tokenIssuer ?? throw new ArgumentNullException(nameof(tokenIssuer));
            _httpClient = httpClient;
             
            // Set information to get Access Token
            _parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("scope", _scope)
            };
        }

        public async Task<OAuthToken> GetTokenAsync()
        {
            // Check if token has been issued or if it has been expired. If true, then make a new request to the Authorization Server
            if (_oauthToken == null || _oauthToken.isExpired())
            {
                // Make Request to Azure AD
                OAuthToken token = await this.RequestAccessToken();
                _oauthToken = token;
            }

            // Return Access Token
            return _oauthToken;
        }

        // This method will make a request to the Authorization Server and return the response which contains the access_token
        private async Task<OAuthToken> RequestAccessToken()
        {
            // Prepare the http post request to get the access token
            HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_tokenIssuer));
            FormUrlEncodedContent requestBody = new FormUrlEncodedContent(_parameters);
            tokenRequest.Content = requestBody;

            // Make request to get the token
            HttpResponseMessage response = await _httpClient.SendAsync(tokenRequest);

            // Get Access Token from the http response
            System.IO.Stream responseStream = await response.Content.ReadAsStreamAsync();

            OAuthToken token = await JsonSerializer.DeserializeAsync<OAuthToken>(responseStream);
            return token;
        }
    }
}