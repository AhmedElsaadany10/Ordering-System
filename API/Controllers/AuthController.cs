using API.Data;
using API.DTOs;
using API.Models;
using API.Repositories.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IOrderRepository _orderRepository;
        public AuthController(AppDbContext context,ITokenService tokenService, IOrderRepository orderRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _orderRepository = orderRepository;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthDto customerDto) {
            if (await _context.Customers.AnyAsync(x => x.Username == customerDto.Username.ToLower()))
                return BadRequest(new { message = "Username already exists." });
            using var hmac=new HMACSHA512();
            var customer = new Customer
            {
                Username = customerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(customerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Add(customer);
            await _context.SaveChangesAsync(); 
            var token = _tokenService.GetToken(customer);
            return Ok(new {token});
        
        }
        [HttpPost("login")]
        public async Task<IActionResult>Login(AuthDto customerDto)
        {
            var customer =await _context.Customers
                .SingleOrDefaultAsync(c => c.Username == customerDto.Username.ToLower());

            if (customer == null)
            return Unauthorized(new { message = "Invalid username" });

            using var hmac=new HMACSHA512(customer.PasswordSalt); 
            var passwordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(customerDto.Password));

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (customer.PasswordHash[i] != passwordHash[i])
                    return Unauthorized(new { message = "Invalid password" });
            }
           
            var token = _tokenService.GetToken(customer);
            var isBan = _orderRepository.IsCustomerBannedAsync(customer.Id).Result;
            
            return Ok(new  { customer.Id,customer.Username, isBan,customer.Role, token});
        }
    }
}
