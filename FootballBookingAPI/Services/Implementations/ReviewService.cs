
 using FootballBookingAPI.Data;
    using FootballBookingAPI.DTOs.Review;
    using FootballBookingAPI.Models;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
namespace FootballBookingAPI.Services.Implementations
{
   

    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(string userId, CreateReviewRequest request)
        {
            var exists = await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.FieldId == request.FieldId);

            if (exists)
                throw new Exception("Already reviewed");

            var review = new Review
            {
                UserId = userId,
                FieldId = request.FieldId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
