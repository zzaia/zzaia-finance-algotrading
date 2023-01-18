using System;

namespace Zzaia.Finance.Core.Models.MarketAgregate
{
    public class Fee
    {
        public Fee() { }
        public Fee(decimal flat, decimal percentage, string type = Types.Exchange)
        {
            Type = Types.IsValid(type) ? type : throw new ArgumentException(nameof(type));
            Flat = flat;
            Percentage = percentage;
        }
        public decimal Flat { get; set; }
        public decimal Percentage { get; set; }
        public string Type { get; set; }
        public class Types
        {
            public const string Transfero = "transfero";
            public const string Bank = "bank";
            public const string Network = "network";
            public const string Exchange = "exchange";
            public static bool IsValid(string type)
            {
                return Transfero.Equals(type)
                    || Bank.Equals(type)
                    || Network.Equals(type)
                    || Exchange.Equals(type);
            }
        }
    }
}
