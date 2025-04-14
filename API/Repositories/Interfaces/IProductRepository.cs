using API.DTOs;
using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersForProductAsync(int productsId);
        Task<Product> AddProductAsync(ProductDto productDto);
        Task<Product> EditProductAsync(int id,ProductDto productDto);

        Task DeleteProductAsync(Product product);
        Task<bool> IsProductExistAsync(int productId);


    }
}
