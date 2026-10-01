using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Authentication.DTO;
using RMS.Application.Helpers.JwtHelper;
using RMS.Services.AuthService;
namespace RMS.Host.Controllers.AuthController;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authservice;

    public AuthController(AuthService authservice)
    {
        _authservice = authservice;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto dto)
    {
        var result = await _authservice.LoginAsync(dto);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignupAsync(CreateUserDto dto)
    {
        var userId = JwtHelper.GetUserId(User);

        var result = await _authservice.SignupAsync(dto, userId);

        return Ok(result);
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _authservice.GetAllAsync();
        return Ok(result);
    }
}