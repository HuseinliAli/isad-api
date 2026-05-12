namespace Core.Shared.Options;

public class JwtTokenOptions
{
    public const string SectionName = "JwtTokenOptions";
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecurityKey { get; set; } = default!;
    public int ExpirationInMinutes { get; set; }
}