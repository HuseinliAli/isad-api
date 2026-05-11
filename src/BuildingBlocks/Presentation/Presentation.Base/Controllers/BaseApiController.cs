using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Base.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator =>
        mediator
        ??= HttpContext.RequestServices.GetService<IMediator>()
        ?? throw new InvalidOperationException("IMediator cannot be retrieved from request services.");
}