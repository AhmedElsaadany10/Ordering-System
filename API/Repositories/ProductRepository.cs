using API.Data;
using API.DTOs;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public  async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products=await _context.Products.Include(x=>x.Orders).Select(p=>new ProductDto {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                
            }).ToListAsync();
            return  products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);   
        }

        public async Task<IEnumerable<Order>> GetAllOrdersForProductAsync(int productId)
        {
            return await _context.Orders.Include(p => p.Product).Include(c=>c.Customer)
                .Where(o => o.ProductId == productId).ToListAsync();
        }
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsProductExistAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) 
                return false;
            return true;
        }
    }
}
