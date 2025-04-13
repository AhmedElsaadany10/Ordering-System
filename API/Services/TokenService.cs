using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _config;

        public TokenService(JwtSettings config)
        {
            _config = config;
        }

        public string GetToken(Customer customer)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Name, customer.Username),
            new Claim(ClaimTypes.Role,customer.Role.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(Convert.ToDouble(_config.ExpiresInHours)),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
             
            return tokenHandler.WriteToken(token);
        }
    }
    
}
