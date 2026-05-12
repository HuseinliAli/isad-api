using Microsoft.AspNetCore.Identity;

namespace IdentityService.Entities.Models;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpires { get; set; }
}