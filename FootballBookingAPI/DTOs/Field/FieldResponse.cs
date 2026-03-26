namespace FootballBookingAPI.DTOs.Field
{
    public class FieldResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal PricePerHour { get; set; }
        public string Status { get; set; }
    }
}
