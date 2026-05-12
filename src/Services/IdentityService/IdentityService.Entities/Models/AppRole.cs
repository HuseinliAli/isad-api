using Microsoft.AspNetCore.Identity;

namespace IdentityService.Entities.Models;

public class AppRole : IdentityRole<int>
{
    public string Description { get; set; } = default!;
}