namespace FootballBookingAPI.DTOs.Booking
{
    public class CreateBookingRequest
    {
        public int FieldId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
