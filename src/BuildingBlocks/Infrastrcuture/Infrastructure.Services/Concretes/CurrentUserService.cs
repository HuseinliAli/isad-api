using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Infrastructure.Services.Concretes;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }


    public int UserId
    {
        get
        {
            var value = _contextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier);

            return int.TryParse(value?.Value, out var id) ? id : 0;
        }
    }
}