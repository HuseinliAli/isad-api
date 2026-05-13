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
        public async Task<IActionResult> CreateEnrollment()
        {
            await mediator.Send(new EnrollmentCreateCommandRequest(Guid.NewGuid()));
            // Placeholder for creating an enrollment
            // You would typically receive a DTO as a parameter and send a command to the mediator
            return Ok();
        }
    }
}
