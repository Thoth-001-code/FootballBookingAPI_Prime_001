using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

    using FootballBookingAPI.DTOs.Booking;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    
    using System.Security.Claims;
    
namespace FootballBookingAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        // ================= USER =================

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _service.GetMyBookingsAsync(userId));
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.CancelAsync(id, userId);

            if (!success) return Forbid();
            return Ok();
        }

        // ================= OWNER =================

        [Authorize(Roles = "Owner")]
        [HttpGet("owner")]
        public async Task<IActionResult> GetOwnerBookings()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _service.GetOwnerBookingsAsync(ownerId));
        }

        // ================= ADMIN =================

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
