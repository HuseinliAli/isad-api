using Core.Shared.Options;
using EnrollmentService.Api.Middlewares;
using EnrollmentService.Application;
using EnrollmentService.Persistence;
using EnrollmentService.Persistence.Context;
using Infrastructure.Interceptors;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Concretes;
using Persistence.EntityFramework.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});


builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
await app.Services.ApplyMigrationsAsync<EnrollmentServiceDbContext>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler();
app.Run();
