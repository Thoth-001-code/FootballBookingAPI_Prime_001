namespace FootballBookingAPI.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
