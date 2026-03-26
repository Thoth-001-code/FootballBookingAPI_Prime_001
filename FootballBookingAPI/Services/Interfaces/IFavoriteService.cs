using FootballBookingAPI.DTOs.Favorite;

namespace FootballBookingAPI.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> AddAsync(string userId, int fieldId);
        Task<bool> RemoveAsync(string userId, int fieldId);
        Task<List<FavoriteResponse>> GetMyFavorites(string userId);
    }
}
