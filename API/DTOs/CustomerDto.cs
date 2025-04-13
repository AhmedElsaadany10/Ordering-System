using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
     
        public string Role { get; set; }
        public bool IsBanned { get; set; }
        public string? BannedUntil { get; set; } 
    }
}
