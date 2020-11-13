using AuthenProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenProject.Common
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<MessageReponse> GenerateJWTToken(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null) throw new Exception($"{UserName} is not found");
            var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwts:Key"]));
            var userRoles = await _userManager.GetRolesAsync(user);
            var claim = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.GivenName,user.FirstName),
                    new Claim(ClaimTypes.Surname,user.LastName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,string.Join(";",userRoles)),

                };
            var token = new JwtSecurityToken(
                       claims: claim,
                       expires: DateTime.UtcNow.AddDays(7),
                       signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                   );


            //var token = tokenHandler.CreateToken(tokenDescriptor);
            string TokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return new MessageReponse()
            {
                Message = TokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }
    }

}
