using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagoTrader.Core.Models
{
    public interface ITradeService : ITrade
    {
        Task ExecuteOrders(List<Order> orders);
        Task ExecuteOrder(Order orders);
        Task CancelOrder(Order order);
        Task<Order> GetOrderById(int Id);

    }

}