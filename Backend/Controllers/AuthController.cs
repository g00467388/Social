using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using backend.Models;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = request.Username };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { message = "User registered successfully", userId = user.Id });
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }
[HttpGet("check-auth")]
public IActionResult CheckAuth()
{
    if (User.Identity?.IsAuthenticated == true)
        return Ok(new { authenticated = true });

    return Unauthorized();
}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: true, lockoutOnFailure: false);
            var user = _userManager.FindByNameAsync(request.Username);
            if (result.Succeeded)
                return Ok(new { user.Id});



            return Unauthorized(new { message = "invalid username or password"  });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout successful" });
        }
    }

    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
