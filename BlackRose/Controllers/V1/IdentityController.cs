using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlackRose.Contracts.V1;
using BlackRose.Contracts.V1.Request;
using BlackRose.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityservice)
        {
            _identityService = identityservice;
        }

        [HttpPost (ApiRoutes.Identity.Register)]
        public async Task<IActionResult> RegisterAsync([FromForm]UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password, request.UserName);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors=authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
            //var newUser = new ApplicationUser
            //{
            //    UserName = User.UserName,
            //    Email = User.Email
            //};
            //var createuser = await _userManager.CreateAsync(newUser, User.Password);

            //if (!createuser.Succeeded)
            //{
            //    return new AuthenticationResultcs
            //    {
            //        ErrorMessage = createuser.Errors.Select(x => x.Description)
            //    };
            //}

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes("s6c5mMwqNk56LhCIJNjXCMfn8Vpk847M");
            //var tokenDescriptior = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
            //        new Claim("id", newUser.Id)
            //    }),
            //    Expires = DateTime.UtcNow.AddHours(2),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            //};
            //var token = tokenHandler.CreateToken(tokenDescriptior);
            //return new AuthenticationResultcs
            //{
            //    Success = true,
            //    Token = tokenHandler.WriteToken(token)
            //};
        }

    }
}