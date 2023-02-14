using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapinew.Models;

using Microsoft.IdentityModel.Tokens;

namespace webapinew.Services
{
    public static class TokenService
    {
        public static string GenerateUserToken(PublicUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.name.ToString()),
                    new Claim(ClaimTypes.Role, user.role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static PublicUser DecodeUserToken(string _bearertoken)
        {

            string[] token = _bearertoken.Split(' ');

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token[1]);
            var TokenInfo = jwtSecurityToken.Claims.ToList();

            var sid = TokenInfo[0].Value;
            var name = TokenInfo[1].Value;
            var role = TokenInfo[2].Value;

            PublicUser user = new PublicUser
            {
                id = Int32.Parse(sid),
                name = name,
                role = role
            };

            return user;
        }
    }
}