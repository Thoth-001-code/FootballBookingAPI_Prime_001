namespace FootballBookingAPI.DTOs.Field
{
    public class UpdateFieldRequest
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
