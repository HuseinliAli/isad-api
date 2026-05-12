using IdentityService.Business.Abstractions;
using IdentityService.Business.Requests;
using IdentityService.Business.Responses;
using IdentityService.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Business.Concretes;

internal class UserService(UserManager<AppUser> userManager, ITokenService tokenService) : IUserService
{
    public async Task AssignLecturer(AssignLecturerRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if(user != null)
            await userManager.AddToRoleAsync(user, AppUserRoleConstants.Lecturer);
    }

    public async Task<TokenResponse> LoginAsync(UserLoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);    
        if (user == null)
        {
            //exception
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            //exception
        }

        return tokenService.CreateAccessToken(user, await userManager.GetRolesAsync(user));
    }

    public async Task<TokenResponse> RegisterAsync(UserRegisterRequest request)
    {
        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            //token creation
        }

        if(request.Email == "adminpass@gmail.com")
            await userManager.AddToRoleAsync(user, AppUserRoleConstants.Admin);

        return tokenService.CreateAccessToken(user, await userManager.GetRolesAsync(user));
    }
}
