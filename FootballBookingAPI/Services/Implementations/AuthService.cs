using FootballBookingAPI.DTOs.Auth;
using FootballBookingAPI.Helpers;
using FootballBookingAPI.Models;
using FootballBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace FootballBookingAPI.Services.Implementations
{
 
 
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHelper _jwtHelper;

        public AuthService(UserManager<ApplicationUser> userManager, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception("Register failed");

            // 🎯 VALIDATE ROLE
            var role = request.Role?.ToLower();

            if (role != "owner")
                role = "user";

            var finalRole = role == "owner" ? "Owner" : "User";

            // 🎯 ADD ROLE
            await _userManager.AddToRoleAsync(user, finalRole);

            // 🎯 TOKEN ĐÚNG ROLE
            var token = _jwtHelper.GenerateToken(user.Id, user.Email, finalRole);

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = finalRole
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var token = _jwtHelper.GenerateToken(user.Id, user.Email, role);

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = role
            };
        }
    }
}
