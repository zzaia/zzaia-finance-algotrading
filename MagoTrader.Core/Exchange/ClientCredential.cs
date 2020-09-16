﻿
namespace MarketMaker.Core.Exchange
{
    public class ClientCredential
    {
        public ExchangeNameEnum? Name { get; set; }
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Twofa { get; set; }

    }
}
