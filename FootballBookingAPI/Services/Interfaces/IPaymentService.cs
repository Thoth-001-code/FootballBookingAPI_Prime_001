using FootballBookingAPI.DTOs.Payment;

namespace FootballBookingAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> PayAsync(string userId, PaymentRequest request);
    }
}
