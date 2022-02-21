namespace MarketIntelligency.Core.Models.OrderBookAggregate
{
    public class OrderBookLevel
    {
        public OrderBookLevel(string id, decimal price, decimal amount)
        {
            Id = id;
            Price = price;
            Amount = amount;
        }
        public string Id { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
