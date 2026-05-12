using IdentityService.Business.Requests;
using IdentityService.Business.Responses;

namespace IdentityService.Business.Abstractions;

public interface IUserService
{
    Task<TokenResponse> RegisterAsync(UserRegisterRequest request);
    Task<TokenResponse> LoginAsync(UserLoginRequest request);
    Task AssignLecturer(AssignLecturerRequest request);
}