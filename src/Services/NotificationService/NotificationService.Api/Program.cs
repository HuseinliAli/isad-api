using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Hubs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.Services.SubscribeToEvents();
app.MapHub<PaymentNotificationHub>("/hubs/payment");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
