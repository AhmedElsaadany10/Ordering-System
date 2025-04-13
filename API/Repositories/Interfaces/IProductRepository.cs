using API.DTOs;
using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersForProductAsync(int productsId);
        Task AddProductAsync(Product product);
        void DeleteProductAsync(Product product);
        Task<bool> IsProductExistAsync(int productId);

        Task<bool> SaveChangesAsync();

    }
}
