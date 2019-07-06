using BlackRose.Domain;
using BlackRose.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace WebAPI.Controllers
{
    public class IdentityService: IIdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtsettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtsettings;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password, string userName)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
         if(existingUser!=null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User With this email address exists" }
                };
           }
            var newUser = new IdentityUser()
            {
                UserName = userName,
                Email = email,
            };

            var createuser = await _userManager.CreateAsync(newUser, password);

            if (!createuser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createuser.Errors.Select(x => x.Description)
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = GenerateToken(newUser);
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = GenerateToken(user);
                return new AuthenticationResult
                {
                    Success = true,
                    Token = tokenHandler.WriteToken(token)
                };
            }
            else
            return new AuthenticationResult
            {
                Errors = new[] { "Username or password is incorrect." }
            };
        }

        public SecurityToken GenerateToken(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            return tokenHandler.CreateToken(tokenDescriptior);
        }
    }
}
