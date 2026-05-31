using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
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

        //post api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //Add roles to the User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User is Register. Please Login");
                    }
                }
            }

            return BadRequest();

        }

        //post api/auth/Login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {

           var identityUser = await userManager.FindByNameAsync(loginRequestDto.UserName);
            if(identityUser != null)
            {
               bool checkpassword = await userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);

                if(checkpassword)
                {
                     var roles = await userManager.GetRolesAsync(identityUser);
                    //Create Token and return to client
                    if (roles != null)
                    {
                        var jwtToken =  tokenRepository.CreateJwtToken(identityUser,roles.ToList());
                        return Ok(jwtToken);
                    }
                }

            }
            return BadRequest("Username or password is incorrect");
        }
    }
}
