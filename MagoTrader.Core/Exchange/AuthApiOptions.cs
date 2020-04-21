
namespace MagoTrader.Core.Exchange
{
    public class AuthApiOptions
    {
        public ExchangeNameEnum? Name { get; set; }
        public string SecretKey { get; set; }
        public string ConnectionKey { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Twofa { get; set; }

    }
}
