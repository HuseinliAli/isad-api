using Core.Shared.Options;
using IdentityService.Business.Abstractions;
using IdentityService.Business.Responses;
using IdentityService.Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Business.Concretes;

public class TokenService : ITokenService
{
    private readonly JwtTokenOptions options;

    public TokenService(IOptions<JwtTokenOptions> options)
    {
        this.options = options.Value;
    }

    public TokenResponse CreateAccessToken(AppUser user, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(this.options.SecurityKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var expires = DateTime.UtcNow.AddMinutes(this.options.ExpirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: this.options.Issuer,
            audience: this.options.Audience,
            claims: claims,
            expires: expires,
            notBefore: DateTime.UtcNow,
            signingCredentials: creds
        );

        return new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires,
            RefreshToken = CreateRefreshToken()
        };
    }

    private string CreateRefreshToken()
    {
        var number = new byte[64];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(number);

        return Convert.ToBase64String(number);
    }
}