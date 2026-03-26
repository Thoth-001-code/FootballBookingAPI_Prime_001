namespace FootballBookingAPI.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> ToggleAsync(string userId, int fieldId);
    }
}
