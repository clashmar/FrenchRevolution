using System.Security.Claims;
using System.Text;
using FrenchRevolution.Application.Config;
using FrenchRevolution.Infrastructure.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FrenchRevolution.Application.Auth.Services;

public interface ITokenService
{
    string CreateTokenForUser(ApplicationUser user, IList<string> roles);
}

public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string CreateTokenForUser(ApplicationUser user, IList<string> roles)
    {
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r))
        ];

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            IssuedAt = DateTime.UtcNow,
        };

        var tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(tokenDescriptor);
    }
}