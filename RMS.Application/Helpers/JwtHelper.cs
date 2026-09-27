using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RMS.Application.Helpers;

public class JwtHelper
{
    private string Key = "Kingofkinginkinglandofnewsintheislandofgold";
    private string Issuer = "RMS";
    private string Audience = "RMS";
    private int ExpiryDays = 365;

    public string GenerateToken(int userId, string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {

            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(ExpiryDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = Issuer,
            ValidAudience = Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Key)),

            ClockSkew = TimeSpan.Zero
        };
    }

    public int? GetUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (int.TryParse(userId, out var id))
        {
            return id;
        }

        return null;
    }
}

