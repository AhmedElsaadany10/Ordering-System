using System.Security.Claims;

namespace API.Extensions
{
    public static class GetCustomerExtension
    {
        public static int GetCustomerId(this ClaimsPrincipal customer)
        {
            return int.Parse(customer.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        public static string GetCustomerUsername(this ClaimsPrincipal customer) 
        { 
            return customer.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
