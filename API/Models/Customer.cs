using API.Enums;

namespace API.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = UserRole.Customer.ToString();
        public bool IsBanned { get; set; } = false;

        public DateTime? BannedUntil { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
