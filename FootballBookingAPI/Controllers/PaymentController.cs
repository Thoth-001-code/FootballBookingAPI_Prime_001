using FootballBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [Authorize(Roles = "User")]
        [HttpPost("{bookingId}")]
        public async Task<IActionResult> Pay(int bookingId)
        {
            return Ok(await _service.PayAsync(bookingId));
        }
    }
}
