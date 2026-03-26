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

        public async Task<PaymentResponse> PayAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            if (booking.Payment != null)
                throw new Exception("Already paid");

            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalPrice,
                Status = PaymentStatus.Paid
            };

            _context.Payments.Add(payment);

            booking.Status = BookingStatus.Confirmed;

            await _context.SaveChangesAsync();

            return new PaymentResponse
            {
                Id = payment.Id,
                BookingId = bookingId,
                Amount = payment.Amount,
                Status = payment.Status.ToString()
            };
        }
    }
}
