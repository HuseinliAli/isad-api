namespace IdentityService.Business.Responses;

public class TokenResponse
{
    public string AccessToken { get; set; } = default!;
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; } = default!;
}