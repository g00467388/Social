using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
 
    private readonly SignInManager<User> _signinManager;
    private readonly UserManager<User> _userManager;
    public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
 
        _signinManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Logout([FromHeader] UserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            return BadRequest("Missing username or password field");

       var result = await _signinManager.PasswordSignInAsync(user.Username, user.Password, true, false );
       
       if (result.Succeeded)
            return Ok(new {Message = $"Logged in as {user.Username}"});

        return Unauthorized();

    }

    [HttpPost("signup")]
    public async Task<ActionResult> Signup([FromHeader] UserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Username))
            return BadRequest("Missing username or password field");
        
        var createdAccount = new User
        {
            UserName = user.Username,
        };
        await _userManager.CreateAsync(createdAccount, user.Password);
        return Ok(new { Message = $"Created account {user.Username}"});
    }

}
