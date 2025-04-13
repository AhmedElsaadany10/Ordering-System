using API.Data;
using API.DTOs;
using API.Enums;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(c=>c.Customer).Include(x=>x.Product).ToListAsync();
               
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _context.Customers.Where(x=>x.Role!=UserRole.Admin.ToString())
                .Select(c => new 
                {
                    c.Id,
                    c.Username,
                    c.Role,
                    c.BannedUntil,
                  
                })
                .ToListAsync();
            var result = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Username = c.Username,
                Role = c.Role,
                BannedUntil = c.BannedUntil.HasValue && c.BannedUntil.Value != DateTime.MinValue
                ? c.BannedUntil.Value.ToString("ddd dd MMM yyyy hh:mm tt")
        : ""
            }).ToList();

            return result;
        }
       
        public async Task<IEnumerable<ProductOrderDto>> GetOrdersByCustomerIdAsync(int customerId)
        {
           return await _context.Orders.Include(x=>x.Product)
                .Where(x => x.CustomerId == customerId).Select(o=>new ProductOrderDto
                {
                    OrderId = o.Id,
                    CustomerId = o.CustomerId,
                    Name=o.Product.Name,
                    Description=o.Product.Description,
                    Price = o.Product.Price,
                    CreatedAt=o.CreatedAt.ToString("ddd dd MMM yyyy"),
                }).ToListAsync();
        }

       
    }
}
