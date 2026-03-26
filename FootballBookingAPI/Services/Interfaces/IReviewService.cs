using FootballBookingAPI.DTOs.Review;

namespace FootballBookingAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<bool> CreateAsync(string userId, CreateReviewRequest request);
    }
}
