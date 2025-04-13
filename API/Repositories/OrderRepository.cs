using API.Data;
using API.DTOs;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            var orderFound = await _context.Orders
                .FirstOrDefaultAsync(x => x.CustomerId == orderDto.CustomerId && x.ProductId == orderDto.ProductId);
            if (orderFound != null)
                return null;

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                ProductId = orderDto.ProductId,
                CreatedAt  = DateTime.Now
                
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<bool> IsOrderAddedBefore(int customerId, int productId)
        {
            return await _context.Orders
                .AnyAsync(o => o.CustomerId == customerId && o.ProductId == productId);
        }
        public async Task<bool> IsProductExistAsync(int productId)
        {
            var product= await _context.Products.FindAsync(productId);
            if (product == null) return false;
            return true;

        }

        public async Task<IEnumerable<Order>> GetAllOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders.Include(p=>p.Product)
                .Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<bool> DeleteOrderAsync(int customerId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Id == orderId);
            if (order == null)
                return false;
            _context.Orders.Remove(order);
            _context.DeletedOrders.Add(new DeletedOrder
            { CustomerId = customerId,
                OrderId = orderId });
            _context.SaveChanges();
            return true;
        }
        public async Task<int> DeletedOrdersCountAsync(int customerId)
        {
            var today = DateTime.Today;
            return await _context.DeletedOrders.
                CountAsync(x => x.CustomerId == customerId && x.DeletedAt.Date == today);
        }
        public async Task BanCustomerAsync(int customerId)
        {
          
            var customer =await _context.Customers.FindAsync(customerId);
            if (customer != null)
            {
                customer.BannedUntil = DateTime.Now.AddMinutes(10);
                customer.IsBanned = true;

            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsCustomerBannedAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            return customer.IsBanned && customer.BannedUntil > DateTime.UtcNow;
        }

       
    }
}
