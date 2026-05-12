using Core.Shared.Options;
using IdentityService.Business;
using IdentityService.DataAccess;
using IdentityService.DataAccess.Context;
using IdentityService.Entities.Models;
using Persistence.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();

    builder.Services.AddOptions<JwtTokenOptions>()
        .BindConfiguration(JwtTokenOptions.SectionName);

    builder.Services.AddJwtAuthentication(builder.Configuration);

    builder.Services.AddBusinessServices();
    builder.Services.AddDataAccessServices(builder.Configuration)
        .AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<IdentityServiceDbContext>();

    builder.Services.AddOpenApi();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
await app.Services.ApplyMigrationsAsync<IdentityServiceDbContext>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
