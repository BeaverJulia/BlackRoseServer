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
        public async Task<IActionResult> RegisterAsync([FromBody]UserRegistrationRequest request)
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
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                UserName = authResponse.UserName
            });
        }

    }
}