using Core.Entities;
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

namespace Shared.Services
{
    public interface IJWTService
    {
        public Task<string> GenerateToken(APIUser user);
        public Task<string> GenerateToken(APIUser user, DateTime dateTime);
        public Task<string> GenerateRandomToken();

    }
    public class JWTService : IJWTService
    {
        private readonly UserManager<APIUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;


        public JWTService(UserManager<APIUser> userManager, IConfiguration configuration, RoleManager<Role> roleManager)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<string> GenerateToken(APIUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var userRole = await _userManager.GetRolesAsync(user);

            string roleslist = string.Join(",", userRole);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),

            };

            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            foreach (var roleName in userRole)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(new Claim(roleClaim.Type, roleClaim.Value));
                }
            }




            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writtenToken = tokenHandler.WriteToken(token);

            return writtenToken;


        }

        public async Task<string> GenerateToken(APIUser user, DateTime dateTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var userRole = await _userManager.GetRolesAsync(user);
            string roleslist = string.Join(",", userRole);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),

            };

            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            foreach (var roleName in userRole)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(new Claim(roleClaim.Type, roleClaim.Value));
                }
            }



            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = dateTime,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writtenToken = tokenHandler.WriteToken(token);

            return writtenToken;


        }


        public async Task<string> GenerateRandomToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Payments"),
                    new Claim(ClaimTypes.Role, "Admin"),
                   
                    
                }),
                Expires = DateTime.UtcNow.AddYears(10),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writtenToken = tokenHandler.WriteToken(token);

            return writtenToken;


        }

    }
}
