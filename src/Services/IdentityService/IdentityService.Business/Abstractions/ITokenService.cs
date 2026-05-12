using IdentityService.Business.Responses;
using IdentityService.Entities.Models;

namespace IdentityService.Business.Abstractions;

public interface ITokenService
{
    TokenResponse CreateAccessToken(AppUser user, IList<string> roles);
}