
using Zzaia.Finance.Core.Models.EnumerationAggregate;

namespace Zzaia.Finance.Core.Models.ExchangeAggregate
{
    public class ClientCredential
    {
        public ExchangeName? Name { get; set; }
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Twofa { get; set; }

    }
}
