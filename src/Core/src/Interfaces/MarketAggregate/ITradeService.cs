using Zzaia.Finance.Core.Models.MarketAgregate;
using Zzaia.Finance.Core.Models.OrderAgregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zzaia.Finance.Core.Interfaces.MarketAggregate
{
    public interface ITradeService
    {
        Task ExecuteOrders(List<Order> orders);
        Task ExecuteOrder(Order orders);
        Task CancelOrder(Order order);
        Task<Order> GetOrderById(int Id);
    }
}