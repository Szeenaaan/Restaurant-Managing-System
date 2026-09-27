using Microsoft.AspNetCore.Mvc;
using RMS.Application.Authentication.DTOs;
using RMS.Application.Helpers;
using RMS.Services.AuthService;
namespace RMS.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authservice;
    private readonly JwtHelper _jwt;

    public AuthController(AuthService authservice, JwtHelper jwt)
    {
        _jwt = jwt;
        _authservice = authservice;
    }

    [HttpPost("login")]
    public async Task <IActionResult> Login(LoginDto dto)
    {
        var result = await _authservice.LoginAsync(dto);

        return Ok(result);
    }
    [HttpPost("signup")]
    public async Task <IActionResult> Signup(CreateUserDto dto)
    {
        var userId = _jwt.GetUserId(User);

        var result = await _authservice.SignupAsync(dto, userId);

        return Ok(result);
    }
}