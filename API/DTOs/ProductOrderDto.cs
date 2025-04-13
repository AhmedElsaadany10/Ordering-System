namespace API.DTOs
{
    public class ProductOrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedAt { get; set; }

    }
}
