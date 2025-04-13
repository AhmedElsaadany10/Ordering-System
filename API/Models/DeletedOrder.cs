namespace API.Models
{
    public class DeletedOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.Now;
    }
}
