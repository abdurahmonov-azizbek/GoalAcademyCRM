using GoalAcademyCRM.Api.Models.Users;
using GoalAcademyCRM.Api.Services.Helpers.Auth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoalAcademyCRM.Api.Services.Helpers.Auth
{
    public class TokenGeneratorService(IOptions<JwtSettings> jwtSettings) : ITokenGeneratorService
    {
        private readonly JwtSettings _settings = jwtSettings.Value;
        public string GenerateToken(User user)
        {
            var claims = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _settings.ValidIssuer,
                audience: _settings.ValidAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_settings.LifeTimeInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private List<Claim> GetClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
        }
    }
}