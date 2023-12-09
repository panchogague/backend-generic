using Chanchas.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chanchas.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenService(IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));
            _userManager = userManager;
        }
        public async Task<string> CreateToken(IdentityUser user)
        {
            var claims = new List<Claim>{
                new(JwtRegisteredClaimNames.NameId,user.Id),
                new(JwtRegisteredClaimNames.UniqueName,user.NormalizedEmail!),
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}