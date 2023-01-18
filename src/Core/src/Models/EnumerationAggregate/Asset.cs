using System;

namespace Zzaia.Finance.Core.Models.EnumerationAggregate
{
    /*  ========================================================================================================
        Supported Assets
        ======================================================================================================= */
    public class Asset : Enumeration
    {
        public static readonly Asset BRL = new Asset(1, "BRL", "Real", Types.Fiat);
        public static readonly Asset BRZ = new Asset(2, "BRZ", "Real", Types.StableCoin);
        public static readonly Asset BTC = new Asset(3, "BTC", "Bitcoin", Types.CryptoCurrency);
        public static readonly Asset ETH = new Asset(4, "ETH", "Ether", Types.CryptoCurrency);
        public static readonly Asset XRP = new Asset(5, "XRP", "Ripple", Types.CryptoCurrency);
        public static readonly Asset LTC = new Asset(6, "LTC", "Lite Coin", Types.CryptoCurrency);
        public static readonly Asset BCH = new Asset(7, "BCH", "Bitcoin Cash", Types.CryptoCurrency);
        public static readonly Asset EOS = new Asset(8, "EOS", "EOS", Types.CryptoCurrency);
        public static readonly Asset USD = new Asset(9, "USD", "Dollar", Types.Fiat);
        public static readonly Asset USDC = new Asset(10, "USDC", "Dollar", Types.StableCoin);
        public static readonly Asset CHZ = new Asset(11, "CHZ", "Chiliz", Types.UtilityToken);
        public static readonly Asset WBX = new Asset(12, "WBX", "Wibx", Types.UtilityToken);
        public static readonly Asset PAXG = new Asset(13, "PAXG", "Pax Gold", Types.StableCoin);
        public static readonly Asset USDT = new Asset(14, "USDT", "Dollar Tether", Types.StableCoin);
        public Asset() { }

        private Asset(int value, string displayName, string name, string type) : base(value, displayName)
        {
            _name = name;
            _type = type;
        }
        private readonly string _name;
        private readonly string _type;
        public string Name
        {
            get { return _name; }
        }
        public string Type
        {
            get { return _type; }
        }
        public class Types
        {
            public const string CryptoCurrency = "crypto currency";
            public const string UtilityToken = "utility token";
            public const string Fiat = "fiat";
            public const string StableCoin = "stablecoin";
            public static bool IsValid(string type)
            {
                return CryptoCurrency.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Fiat.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || StableCoin.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || UtilityToken.Equals(type, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
