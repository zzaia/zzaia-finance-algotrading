using System;

namespace MarketIntelligency.WebGrpc.Authentication
{
    public class OAuth2Options
    {
        public string OptionName { get; set; }
        public string Issuer { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }

        public Uri GetIssuerUri()
        {
            return new Uri(Issuer, UriKind.Absolute);
        }

        public bool TryGetIssuerUri(out Uri resultUri)
        {
            return Uri.TryCreate(Issuer, UriKind.Absolute, out resultUri);
        }

        public string GetComposedScope()
        {
            return String.Format(Scope, ClientId);
        }

        public string GetComposedScope(string clientId)
        {
            return String.Format(Scope, clientId);
        }
    }
}