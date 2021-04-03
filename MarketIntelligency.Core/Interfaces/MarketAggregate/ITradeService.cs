using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderAgregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Interfaces.MarketAggregate
{
    public interface ITradeService
    {
        Task ExecuteOrders(List<Order> orders);
        Task ExecuteOrder(Order orders);
        Task CancelOrder(Order order);
        Task<Order> GetOrderById(int Id);
    }
}