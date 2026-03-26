 using Microsoft.AspNetCore.Identity;
    using static FootballBookingAPI.Models.Enums.Enums;
    using static System.Net.WebRequestMethods;

namespace FootballBookingAPI.Models
{
   

    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Field> Fields { get; set; } = new List<Field>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
