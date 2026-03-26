namespace FootballBookingAPI.DTOs.Booking
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
