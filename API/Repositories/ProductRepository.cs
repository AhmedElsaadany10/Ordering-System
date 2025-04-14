using API.Data;
using API.DTOs;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public  async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(x => x.Orders).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);   
        }
        //get all orders for product
        public async Task<IEnumerable<Order>> GetAllOrdersForProductAsync(int productId)
        {
            return await _context.Orders.Include(p => p.Product).Include(c=>c.Customer)
                .Where(o => o.ProductId == productId).ToListAsync();
        }
        public async Task<Product> AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
          return product;
        }
        public async Task<Product> EditProductAsync(int id,ProductDto productDto)
        {
            var product =await _context.Products.FindAsync(id);
            if (product == null)
                return null;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
           await _context.SaveChangesAsync();
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
