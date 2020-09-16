namespace MarketMaker.Core.Models
{
    /*  ========================================================================================================
            Supported Assets
        ======================================================================================================= */
    public class AssetTicker : Enumeration
    {
        public static readonly AssetTicker BRL = new AssetTicker(1, "BRL");
        public static readonly AssetTicker BRZ = new AssetTicker(2, "BRZ");
        public static readonly AssetTicker BTC = new AssetTicker(3, "BTC");
        public static readonly AssetTicker ETH = new AssetTicker(4, "ETH");
        public static readonly AssetTicker XRP = new AssetTicker(5, "XRP");
        public static readonly AssetTicker LTC = new AssetTicker(6, "LTC");
        public static readonly AssetTicker BCH = new AssetTicker(7, "BCH");
        public static readonly AssetTicker EOS = new AssetTicker(8, "EOS");
        public static readonly AssetTicker USD = new AssetTicker(9, "USD");
        public static readonly AssetTicker USDC = new AssetTicker(10, "USDC");
        public AssetTicker() { }

        private AssetTicker(int value, string displayName) : base(value, displayName) { }
    }
}
