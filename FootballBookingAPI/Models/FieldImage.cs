namespace FootballBookingAPI.Models
{
    public class FieldImage
    {
        public int Id { get; set; }

        public int FieldId { get; set; }
        public Field Field { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
