using static FootballBookingAPI.Models.Enums.Enums;

namespace FootballBookingAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int FieldId { get; set; }
        public Field Field { get; set; } = null!;

        // ================= CHANGE =================
        public DateTime BookingDate { get; set; }      // chỉ ngày
        public TimeSpan StartTime { get; set; }       // giờ bắt đầu
        public TimeSpan EndTime { get; set; }         // giờ kết thúc

        public decimal TotalPrice { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Payment? Payment { get; set; }
    }
}
