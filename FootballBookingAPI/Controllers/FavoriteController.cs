using FootballBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballBookingAPI.Controllers
{
    [ApiController]
    [Route("api/favorite")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _service;

        public FavoriteController(IFavoriteService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("{fieldId}")]
        public async Task<IActionResult> Add(int fieldId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.AddAsync(userId, fieldId);
            if (!result) return BadRequest("Already exists");
            return Ok();
        }

        [Authorize]
        [HttpDelete("{fieldId}")]
        public async Task<IActionResult> Remove(int fieldId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.RemoveAsync(userId, fieldId);
            if (!result) return NotFound();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _service.GetMyFavorites(userId));
        }
    }
}
