using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BearingManagementApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginUserAsync(request);
        if (response != null) return Ok(response);
        return Unauthorized("Invalid username or password");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var response = await _authService.RegisterUser(model);
        if (response) return Ok();
        return BadRequest();
    }
}