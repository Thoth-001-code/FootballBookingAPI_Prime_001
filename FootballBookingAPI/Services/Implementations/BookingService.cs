
 using FootballBookingAPI.Data;
    using FootballBookingAPI.DTOs.Booking;
    using FootballBookingAPI.Models;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using static FootballBookingAPI.Models.Enums.Enums;


namespace FootballBookingAPI.Services.Implementations
{
   

    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookingResponse> CreateAsync(string userId, CreateBookingRequest request)
        {
            // 1. Validate time
            if (request.EndTime <= request.StartTime)
                throw new Exception("EndTime must be greater than StartTime");

            // 2. Check field exists
            var field = await _context.Fields.FindAsync(request.FieldId);
            if (field == null || field.Status != FieldStatus.Approved)
                throw new Exception("Field not available");

            // 3. CHECK TRÙNG LỊCH 🔥
            var isConflict = await _context.Bookings
                .AnyAsync(b =>
                    b.FieldId == request.FieldId &&
                    b.BookingDate.Date == request.BookingDate.Date &&
                    b.Status != BookingStatus.Cancelled &&
                    request.StartTime < b.EndTime &&
                    request.EndTime > b.StartTime
                );

            if (isConflict)
                throw new Exception("Time slot already booked");

            // 4. Tính tiền
            var hours = (decimal)(request.EndTime - request.StartTime).TotalHours;
            var totalPrice = hours * field.PricePerHour;

            // 5. Create booking
            var booking = new Booking
            {
                FieldId = request.FieldId,
                UserId = userId,
                BookingDate = request.BookingDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return new BookingResponse
            {
                Id = booking.Id,
                FieldId = booking.FieldId,
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToString()
            };
        }

        public async Task<List<BookingResponse>> GetMyBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    FieldId = b.FieldId,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<BookingResponse>> GetAllAsync()
        {
            return await _context.Bookings
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    FieldId = b.FieldId,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<BookingResponse>> GetOwnerBookingsAsync(string ownerId)
        {
            return await _context.Bookings
                .Include(b => b.Field)
                .Where(b => b.Field.OwnerId == ownerId)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    FieldId = b.FieldId,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<bool> CancelAsync(int bookingId, string userId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.UserId != userId)
                return false;

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
