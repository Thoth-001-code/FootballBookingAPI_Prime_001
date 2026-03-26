using FootballBookingAPI.DTOs.Auth;
namespace FootballBookingAPI.Services.Interfaces
{


    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
