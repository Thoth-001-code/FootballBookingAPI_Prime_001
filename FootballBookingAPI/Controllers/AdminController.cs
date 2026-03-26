using FootballBookingAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FootballBookingAPI.Models.Enums.Enums;

namespace FootballBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalBookings = await _context.Bookings.CountAsync();
            var revenue = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Paid)
                .SumAsync(p => p.Amount);

            return Ok(new
            {
                totalUsers,
                totalBookings,
                revenue
            });
        }
    }
}
