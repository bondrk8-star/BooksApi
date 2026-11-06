using BooksApi.Server.Interfaces;
using BooksApi.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BooksApi.Server.Services
{
    public class TokenService : ITokenService
    {
        public TokenService(IOptions<TokenServiceOptions> options)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        }

        private readonly SymmetricSecurityKey _key;

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new (JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(4),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
