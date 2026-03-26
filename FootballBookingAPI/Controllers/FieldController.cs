   using FootballBookingAPI.DTOs.Field;
using FootballBookingAPI.Models;
    using FootballBookingAPI.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
    
    using System.Security.Claims;
namespace FootballBookingAPI.Controllers
{
 


    [ApiController]
    [Route("api/[controller]")]
    public class FieldController : ControllerBase
    {
        private readonly IFieldService _service;
        private readonly IFieldService _fieldService;
        public FieldController(IFieldService service, IFieldService fieldService)
        {
            _service = service;
            _fieldService = fieldService;
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
        // owner cập nhật sân, chỉ cập nhật khi sân đang pending hoặc đã bị reject
        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFieldRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.UpdateAsync(id, userId, request);

            if (!success) return Forbid();
            return Ok();
        }
        // owner xóa sân, chỉ xóa khi chưa có booking nào hoặc tất cả booking đều đã hủy
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
        // admin reject sân
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _service.RejectAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
        // admin xem tất cả sân, kể cả pending/rejected
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            return Ok(await _service.GetAllForAdminAsync());
        }
        // ================= OTHER =================
        [Authorize(Roles = "Owner")]
        [HttpGet("my-fields")]
        public async Task<IActionResult> GetMyFields()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _service.GetMyFieldsAsync(userId));
        }
        // tìm kiếm sân theo keyword, location, price range, pagination, sorting
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] FieldQueryRequest request)
        {
            var result = await _fieldService.SearchAsync(request);
            return Ok(result);
        }

    }
}
