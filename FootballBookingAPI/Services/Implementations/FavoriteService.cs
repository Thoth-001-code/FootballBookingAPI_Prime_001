namespace FootballBookingAPI.Services.Implementations
{
    using FootballBookingAPI.Data;
    using FootballBookingAPI.DTOs.Favorite;
    using FootballBookingAPI.Models;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(string userId, int fieldId)
        {
            var exists = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.FieldId == fieldId);

            if (exists) return false;

            _context.Favorites.Add(new Favorite
            {
                UserId = userId,
                FieldId = fieldId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(string userId, int fieldId)
        {
            var fav = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FieldId == fieldId);

            if (fav == null) return false;

            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FavoriteResponse>> GetMyFavorites(string userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => new FavoriteResponse
                {
                    FieldId = f.FieldId,
                    FieldName = f.Field.Name
                })
                .ToListAsync();
        }
    }
}
