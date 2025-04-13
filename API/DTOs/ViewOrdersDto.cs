namespace API.DTOs
{
    public class ViewOrdersDto
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedAt { get; set; }
    }
}
