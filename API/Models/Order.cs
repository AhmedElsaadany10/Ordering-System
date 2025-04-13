using System.Text.Json.Serialization;

namespace API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

    }
}
