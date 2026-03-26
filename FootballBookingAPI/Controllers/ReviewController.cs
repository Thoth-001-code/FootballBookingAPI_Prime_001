using FootballBookingAPI.DTOs.Review;
using FootballBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballBookingAPI.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateAsync(userId, request);
            if (!result) return BadRequest();
            return Ok();
        }

        [HttpGet("{fieldId}")]
        public async Task<IActionResult> GetByField(int fieldId)
        {
            return Ok(await _service.GetByFieldAsync(fieldId));
        }
    }
}
