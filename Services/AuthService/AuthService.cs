using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Services.AuthService;

public class AuthService(IConfiguration configuration) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJWT(string email, string username)
    {
        var issuer = _configuration["JWT:Issuer"];
        var audience = _configuration["JWT:Audience"];
        var key = _configuration["JWT:Key"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            // new() == new Claim()
            new("Email", email),
            new("Username", username),
            new("EmailIdentifier", email.Split("@").ToString()!),
            new("CurrentTime", DateTime.Now.ToString())
        };

        // _ é um operador de descarte da variável "tokenExpirationTimeInDays"
        // No TryParse (valor que quero fazer a conversão, valor de saída caso conversão dê certo)
        _ = int.TryParse(_configuration["JWT:TokenExpirationTimeInDays"], out int tokenExpirationTimeInDays);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddDays(tokenExpirationTimeInDays),
            signingCredentials: credentials);

        var tokenHanlder = new JwtSecurityTokenHandler();

        return tokenHanlder.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        return Convert.ToBase64String(secureRandomBytes);
    }

    public string HashingPassword(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
            // x2 - retorna a representação hexadecimal
        }

        return builder.ToString();
    }
}
