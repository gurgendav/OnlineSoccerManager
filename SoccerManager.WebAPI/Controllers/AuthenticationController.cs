using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Interfaces;
using SoccerManager.WebAPI.Models;

namespace SoccerManager.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.Register(model.Email, model.Password);
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result.Errors);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.Login(model.Email, model.Password);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        // TODO consistent error response models
        return Unauthorized(result);
    }
}