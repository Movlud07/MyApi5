using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.Entities.DTOs.UserDtos;
using MyApi5.Business.concretes;
using Microsoft.AspNetCore.Http.HttpResults;
using MyApi5.Entities.concretes;

namespace MyApi5.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenOption _tokenOptions;
        private readonly IConfiguration _configuration;
        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOption>();
        }
        //[HttpGet]
        //public async Task<ActionResult> CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("User"));
        //    return NoContent();
        //}
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            try
            {
                await _userService.Register(registerDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            //try
            //{
            string token = await _userService.Login(loginDto);
            return Ok(new
            {
                Token = token,
                ExpireDate = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpirationTime)
            });
            //}
            //catch(Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

        }

    }
}
