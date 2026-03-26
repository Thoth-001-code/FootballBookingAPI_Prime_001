namespace FootballBookingAPI.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int FieldId { get; set; }
        public Field Field { get; set; } = null!;

        public int Rating { get; set; } // nên validate 1–5 ở API

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
