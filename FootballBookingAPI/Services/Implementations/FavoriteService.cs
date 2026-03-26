namespace FootballBookingAPI.Services.Implementations
{
    using FootballBookingAPI.Data;
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

        public async Task<bool> ToggleAsync(string userId, int fieldId)
        {
            var fav = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FieldId == fieldId);

            if (fav != null)
            {
                _context.Favorites.Remove(fav);
                await _context.SaveChangesAsync();
                return false;
            }

            _context.Favorites.Add(new Favorite
            {
                UserId = userId,
                FieldId = fieldId
            });

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
