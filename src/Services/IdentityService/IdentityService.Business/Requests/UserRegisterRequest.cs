namespace IdentityService.Business.Requests;

public record UserRegisterRequest(string FirstName,
    string LastName,
    string Email,
    string Password
);