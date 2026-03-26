namespace FootballBookingAPI.Models
{
    public class FieldSchedule
    {
        public int Id { get; set; }

        public int FieldId { get; set; }
        public Field Field { get; set; } = null!;

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
