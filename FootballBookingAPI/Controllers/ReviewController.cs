using FootballBookingAPI.DTOs.Review;
using FootballBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _service.CreateAsync(userId, request);
            return Ok();
        }
    }
}
