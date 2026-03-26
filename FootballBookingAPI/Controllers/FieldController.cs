using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
   using FootballBookingAPI.DTOs.Field;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    
    using System.Security.Claims;
namespace FootballBookingAPI.Controllers
{
 


    [ApiController]
    [Route("api/[controller]")]
    public class FieldController : ControllerBase
    {
        private readonly IFieldService _service;

        public FieldController(IFieldService service)
        {
            _service = service;
        }

        // ================= USER =================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // ================= OWNER =================

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateFieldRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFieldRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.UpdateAsync(id, userId, request);

            if (!success) return Forbid();
            return Ok();
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.DeleteAsync(id, userId);

            if (!success) return Forbid();
            return Ok();
        }

        // ================= ADMIN =================

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _service.ApproveAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _service.RejectAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            return Ok(await _service.GetAllForAdminAsync());
        }
        [Authorize(Roles = "Owner")]
        [HttpGet("my-fields")]
        public async Task<IActionResult> GetMyFields()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _service.GetMyFieldsAsync(userId));
        }
    }
}
