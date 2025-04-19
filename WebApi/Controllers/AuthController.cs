using Business.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpForm formData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.SignUpAsync(formData);
            if (result is null) return Conflict("User already exists");
            return Ok(result);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInForm formData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.SignInAsync(formData);
            if (result is null) return Unauthorized("Invalid Email or Password");

            Response.Cookies.Append("jwt", result.Token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(result);
        }

        [RequireKey("AdminKey", "UserKey")]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutUser()
        {
            await _userService.SignOutAsync();
            Response.Cookies.Delete("jwt");
            return NoContent();
        }
    }
}
