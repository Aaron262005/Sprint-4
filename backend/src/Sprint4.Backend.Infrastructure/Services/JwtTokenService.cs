using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Enums;

namespace Sprint4.Backend.Infrastructure.Services
{
    /// <summary>
    /// Implementación concreta de ITokenService usando JWT.
    /// Si el equipo cambia de mecanismo de autenticación, esta es la ÚNICA clase
    /// que se reemplaza; LoginCommandHandler y AuthController no cambian.
    /// </summary>
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string username, UserRole role)
        {
            var section = _configuration.GetSection(AppConstants.Jwt.SectionName);
            var key = section["Key"] ?? "clave-temporal-de-desarrollo-cambiar-en-produccion";
            var expirationMinutes = int.TryParse(section["ExpirationMinutes"], out var minutes)
                ? minutes
                : AppConstants.Jwt.ExpirationMinutes;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString()),
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
