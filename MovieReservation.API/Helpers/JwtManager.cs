using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

using System.Text;

namespace MovieReservation.API;

public class JwtManager
{
    private readonly IOptions<JwtSettings> _options;
    public JwtManager(IOptions<JwtSettings> settings)
    {
        _options = settings;
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
    
    public string GenerateJWT(UserDTO user)
    {
        //Create user info for the token 
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Name)
            // new Claim(ClaimTypes.Role, user.Roles!.First() ?? string.Empty)
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key!));
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
            (Encoding.UTF8.GetBytes(_options.Value.Key!))
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
}

