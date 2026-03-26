namespace FootballBookingAPI.Models.Enums
{
    public class Enums
    {
        public enum UserStatus
        {
            Active = 0,
            Locked = 1
        }

        public enum FieldStatus
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2
        }

        public enum BookingStatus
        {
            Pending = 0,
            Confirmed = 1,
            Rejected = 2,
            Cancelled = 3
        }

        public enum PaymentStatus
        {
            Pending = 0,
            Paid = 1
        }
    }
}
