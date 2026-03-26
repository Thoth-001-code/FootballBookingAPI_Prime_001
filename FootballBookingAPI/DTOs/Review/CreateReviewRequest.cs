namespace FootballBookingAPI.DTOs.Review
{
    public class CreateReviewRequest
    {
        public int FieldId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
