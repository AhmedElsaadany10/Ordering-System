using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrderDto
    {
        public int CustomerId  { get; set; }
        
        public int ProductId { get; set; }
    }
}
