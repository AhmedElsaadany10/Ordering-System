using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("customers")]
        
        public async Task<IActionResult> GetAllCustomers()
        {
               var customers= await _adminRepository.GetAllCustomersAsync();
           
            return Ok(customers);
        }
        [HttpGet("orders-customer/{id}")]
        
        // get all orders for customer
        public async Task<IActionResult> GetCustomerOrders(int id)
        {
            return Ok(await _adminRepository.GetOrdersByCustomerIdAsync(id));
        }
      
    }
}
