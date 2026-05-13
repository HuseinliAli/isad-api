using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Business.Abstractions;

namespace PaymentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentHistoryService paymentHistoryService) : ControllerBase
    {
        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            return Ok(await paymentHistoryService.PaymentHistoryAsync());
        }
    }
}
