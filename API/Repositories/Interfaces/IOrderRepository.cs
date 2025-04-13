using API.DTOs;
using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(OrderDto orderDto);
        Task<IEnumerable<Order>> GetAllOrdersByCustomerAsync(int customerId);
        Task<bool> DeleteOrderAsync(int customerId, int orderId);
        Task<int> DeletedOrdersCountAsync(int customerId);
        Task BanCustomerAsync(int customerId);
        Task<bool> IsCustomerBannedAsync(int customerId);
        Task<bool> IsOrderAddedBefore(int customerId, int orderId);

    }
}
