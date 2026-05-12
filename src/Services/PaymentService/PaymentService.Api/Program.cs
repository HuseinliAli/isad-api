using PaymentService.Business;
using PaymentService.DataAccess;
using PaymentService.DataAccess.Context;
using Persistence.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.Services.ApplyMigrationsAsync<PaymentServiceDbContext>();    

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.SubscribeToEvents();

app.Run();
