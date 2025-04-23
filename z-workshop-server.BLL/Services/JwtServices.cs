using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using z_workshop_server.BLL.DTOs;

namespace z_workshop_server.BLL.Services;

public interface IJwtServices
{
    string GenerateToken(UserDTO user);
    string? ValidateToken(string token);
}

public class JwtServices : IJwtServices
{
    private IConfiguration _config;

    public JwtServices(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(UserDTO user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        Claim[]? claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            null,
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string? ValidateToken(string? token)
    {
        if (token == null)
            return null;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = _config["Jwt:Issuer"],
                },
                out SecurityToken validatedToken
            );
            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Subject;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
