using API.DTOs;
using API.Extensions;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetOrdersForCustomer()
        {
            var customerId = User.GetCustomerId();
            var orders=await _orderRepository.GetAllOrdersByCustomerAsync(customerId);

            var result = orders.Select(o => new ViewOrdersDto
            {
                OrderId = o.Id,
                ProductName = o.Product.Name,
                Description = o.Product.Description,
                Price = o.Product.Price,
                CreatedAt = o.CreatedAt.ToString("ddd dd MMM ")
            }).ToList();
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            var customerId = User.GetCustomerId();
            var isBanned = await _orderRepository.IsCustomerBannedAsync(customerId);
            if (isBanned)
            return BadRequest(new { message = "you are banned from making order" });


            var productExist =await _productRepository.IsProductExistAsync(orderDto.ProductId);
            if (!productExist)
                return BadRequest(new { message = "product is not found" });

            var OrderAdded=await _orderRepository.IsOrderAddedBefore(customerId, orderDto.ProductId);
            if(OrderAdded)
                return BadRequest(new { message = "You have already ordered this product" });

            var order = await _orderRepository.CreateOrderAsync(orderDto);
            return Ok(new ViewOrdersDto
            {
                OrderId = order.Id,
                ProductName = order.Product.Name,
                Description = order.Product.Description,
                Price = order.Product.Price,
                CreatedAt = order.CreatedAt.ToString("ddd dd MMM")
            });
           
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id) {
            var customerId = User.GetCustomerId();
            var orderDeleted=await _orderRepository.DeleteOrderAsync(customerId,id);
            if (!orderDeleted)
            return BadRequest(new { message = "Order not found" });


            var deletedDeletedOrdersCountByDay =await _orderRepository.DeletedOrdersCountAsync(customerId);
            if(deletedDeletedOrdersCountByDay >= 3)
            {
                await _orderRepository.BanCustomerAsync(customerId);
                return Ok(new { message = "Order deleted, But You have been banned for 6 hours",isBan=true });
            }
            return Ok(new { message = "Order deleted successfully" });
        }
    }
}
