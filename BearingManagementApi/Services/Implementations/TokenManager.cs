using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Models.Options;
using BearingManagementApi.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BearingManagementApi.Services.Implementations;

public class TokenManager(IOptions<TokenOptionsModel> options) : ITokenManager
{
    public string? GenerateToken(User? user)
    {
        var key = Encoding.ASCII.GetBytes(options.Value.JwtToken ?? string.Empty);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        if(user == null) return null;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddHours(options.Value.JwtExpiryHours),
            Issuer = options.Value.JwtIssuer,
            Audience = options.Value.JwtAudience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}