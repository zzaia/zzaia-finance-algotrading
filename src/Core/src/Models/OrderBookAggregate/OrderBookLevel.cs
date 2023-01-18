using System;

namespace Zzaia.Finance.Core.Models.OrderBookAggregate
{
    public class OrderBookLevel
    {
        public OrderBookLevel(decimal price, decimal amount)
        {
            Id = Guid.NewGuid().ToString();
            Price = price;
            Amount = amount;
        }
        public string Id { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
