using GroundHogApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroundHogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            // Process the payment here
            // For example, validate the payment details, store it in a database, etc.

            return Ok(new { message = "Payment processed successfully.", payment });
        }
    }
}
