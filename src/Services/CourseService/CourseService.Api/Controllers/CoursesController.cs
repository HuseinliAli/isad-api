using CourseService.Applcation.Features.Courses.Queries.Paged;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Api.Controllers;

[Route("api/[controller]")]
public class CoursesController : BaseApiController
{
    [Authorize]
    [HttpGet("size/{size:int:min(1)}/page/{number:int:min(1)}")]
    public async Task<IActionResult> Get([FromRoute] int size, [FromRoute]int number)
    {
        var result = await Mediator.Send(new CoursePagedQueryRequest() { PageSize = size, PageNumber = number});
        
        return Ok(result);
    }
}