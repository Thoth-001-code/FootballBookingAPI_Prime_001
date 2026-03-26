namespace FootballBookingAPI.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int FieldId { get; set; }
        public Field Field { get; set; } = null!;
    }
}
