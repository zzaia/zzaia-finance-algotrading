using System;

namespace MarketIntelligency.Web.Grpc.Clients.Authentication
{
    public class OAuthToken
    {
        public OAuthToken()
        {
            created_at = DateTimeOffset.Now;
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public int? expires_in { get; set; }
        public int? ext_expires_in { get; set; }
        public DateTimeOffset created_at { get; set; }

        public bool isExpired()
        {
            DateTimeOffset expires_at = this.created_at.AddSeconds(Convert.ToDouble(this.expires_in));
            return DateTimeOffset.Now.CompareTo(expires_at) >= 0;
        }
    }
}
