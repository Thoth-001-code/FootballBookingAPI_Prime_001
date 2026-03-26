using static FootballBookingAPI.Models.Enums.Enums;

namespace FootballBookingAPI.Models
{
    public class Field
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? Description { get; set; }

        public decimal PricePerHour { get; set; }

        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; set; } = null!;

        public FieldStatus Status { get; set; } = FieldStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<FieldSchedule> Schedules { get; set; } = new List<FieldSchedule>();
        public ICollection<FieldImage> Images { get; set; } = new List<FieldImage>();
    }
}
