 using FootballBookingAPI.Data;
    using FootballBookingAPI.DTOs.Payment;
    using FootballBookingAPI.Models;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using static FootballBookingAPI.Models.Enums.Enums;

namespace FootballBookingAPI.Services.Implementations
{


    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentResponse> PayAsync(string userId, PaymentRequest request)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.UserId == userId);

            if (booking == null) throw new Exception("Booking not found");

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = booking.TotalPrice,
                Status = PaymentStatus.Paid
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new PaymentResponse
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Status = payment.Status.ToString()
            };
        }
    }
}
