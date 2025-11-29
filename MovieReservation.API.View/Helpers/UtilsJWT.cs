using MovieReservation.Model.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieReservation.API.View.Helpers;

public class UtilsJWT
{
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    public UtilsJWT(IConfiguration configuration, IMemoryCache cache)
    {
        _configuration = configuration;
        _cache = cache;
    }

    public string EncryptSHA256(string texto)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public string GenerateJWT(User user)
    {
        //Create user info for the token 
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Roles!.First().Name ?? string.Empty)
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        //Create detail of token
        var jwtConfig = new JwtSecurityToken(
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
    }

    public bool ValidarToken(string token)
    {

        if (IsInBlacklist(token))
            return false;

        var claimsPrincipal = new ClaimsPrincipal();
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!))
        };

        try
        {
            claimsPrincipal= tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void InvalidateToken(string token)
    {
        // Get token expiration date
        var expiracion = GetTokenExpiration(token);
        
        // Save in cache until expire 
        _cache.Set($"blacklist_{token}", true, expiracion);
    }
    
    public bool IsInBlacklist(string token)
    {
        return _cache.TryGetValue($"blacklist_{token}", out _);
    }
    
    private DateTimeOffset GetTokenExpiration(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return DateTimeOffset.FromUnixTimeSeconds(jwtToken.Payload.Expiration ?? 0);
    }
}
