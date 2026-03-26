using FootballBookingAPI.DTOs.Booking;

namespace FootballBookingAPI.Services.Interfaces
{
    


    public interface IBookingService
    {
        Task<BookingResponse> CreateAsync(string userId, CreateBookingRequest request);

        Task<List<BookingResponse>> GetMyBookingsAsync(string userId);

        Task<List<BookingResponse>> GetAllAsync(); // admin

        Task<List<BookingResponse>> GetOwnerBookingsAsync(string ownerId);

        Task<bool> CancelAsync(int bookingId, string userId);
    }
}
