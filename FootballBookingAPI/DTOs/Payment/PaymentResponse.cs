namespace FootballBookingAPI.DTOs.Payment
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
    }
}
