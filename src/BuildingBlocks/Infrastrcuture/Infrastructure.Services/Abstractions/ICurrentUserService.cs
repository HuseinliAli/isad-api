namespace Infrastructure.Services.Abstractions;

public interface ICurrentUserService
{
    int UserId { get; }
}