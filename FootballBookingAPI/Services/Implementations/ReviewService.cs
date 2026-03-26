
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
            if (request.Rating < 1 || request.Rating > 5)
                return false;

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

        public async Task<List<object>> GetByFieldAsync(int fieldId)
        {
            return await _context.Reviews
                .Where(r => r.FieldId == fieldId)
                .Select(r => new
                {
                    r.Rating,
                    r.Comment,
                    r.CreatedAt
                })
                .ToListAsync<object>();
        }
    }
}
