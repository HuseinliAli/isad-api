using EnrollmentService.Application.Features.Enrollments.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentCreateCommandRequest request)
        {
            await mediator.Send(request);
            return Ok();
        }
    }
}
