using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models.DTO;
using MyWebApi.Repositories;

namespace MyWebApi.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //Add  roles to the user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    foreach (var role in registerRequestDto.Roles)
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(identityUser, role);
                        if (!addToRoleResult.Succeeded)
                        {
                            return BadRequest("Failed to add roles to the user.");
                        }
                    }
                }
                return Ok("User was registered successfully! Please Login");
            }
            return BadRequest("Failed to register user.");

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //Get the roles for the user
                    var roles =  await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //Create Token
                        var jwtToken =  tokenRepository.CreateJwtToken(user, roles.ToList());
                        var reponse = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(reponse);
                    }
                }
            }
            return BadRequest("Username Or Password is incorrect");
        }
        
    }
}