using Business.Services;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Documentation;
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
        [SwaggerOperation(Summary = "Register new user")]
        [SwaggerRequestExample(typeof(SignUpForm), typeof(SignUpDataExample))]
        [SwaggerResponseExample(400, typeof(UserValidationErrorExample))]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUp(SignUpForm formData)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.SignUpAsync(formData);
            if (result is null) return Conflict("User already exists");
            return Ok(result);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Sign in user")]
        [SwaggerRequestExample(typeof(SignInForm), typeof(SignInDataExample))]
        [SwaggerResponseExample(401, typeof(SignInErrorExample))]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [SwaggerOperation(Summary = "Sign out current user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignOutUser()
        {
            await _userService.SignOutAsync();
            Response.Cookies.Delete("jwt");
            return NoContent();
        }
    }
}
