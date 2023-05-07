using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        //POST:/api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var IdentityResult = await _userManager.CreateAsync(IdentityUser, registerRequestDto.Password);

            if(IdentityResult.Succeeded)
            {
                //Add roles to this User
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    IdentityResult = await _userManager.AddToRolesAsync(IdentityUser, registerRequestDto.Roles);
                }

                if(IdentityResult.Succeeded)
                {
                    return Ok("User was registered! Please Login.");
                }


            }

            return BadRequest("Something went wrong");
        }

        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if(user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(checkPasswordResult)
                {
                    //Create Token

                    return Ok();
                }
            }

            return BadRequest("Username or password incorrrect");
        }
    }
}
