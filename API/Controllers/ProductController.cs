using API.DTOs;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts() { 
            var products=await _productRepository.GetAllProductsAsync();
            await _productRepository.SaveChangesAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductById(int id) { 
            var orders=await _productRepository.GetAllOrdersForProductAsync(id);
            if (orders == null)
                return NotFound("Order not found");
            var result = orders.Select(o => new ViewProductCustomers
            {
                CustomerName =  o.Customer.Username ,
                CreatedAt = o.CreatedAt.ToString("ddd dd MMM ")
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Addproduct([FromBody]Product product)
        {
           await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody]ProductDto productDto) {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            await _productRepository.SaveChangesAsync();
            return Ok(product);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id) {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            _productRepository.DeleteProductAsync(product);
            await _productRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
