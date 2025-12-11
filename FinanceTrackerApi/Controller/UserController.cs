using FinanceTracker.Models;
using FinanceTrackerApi.Data;
using FinanceTrackerApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTrackerApi.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IJwtService _jwtService;

    public UsersController(UserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    // POST: api/Users/Register
    [HttpPost("Register")]
   public IActionResult Register(User request)
{
    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

    bool success = _userService.RegisterUser(
        request.Username,
        request.Email,
        hashedPassword
    );

    if (!success)
        return BadRequest("User already exists.");

    return Ok("User registered successfully.");
}

    
// POST: api/Users/Login
    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Invalid login data.");

        if (!_userService.ValidateUserCredentials(request.Email, request.Password, out var user) || user == null)
            return Unauthorized("Invalid credentials.");

        // Generate tokens
        var tokens = _jwtService.GenerateTokens(user);

        // Save refresh token & expiry in DB
        var refreshExpiry = DateTime.UtcNow.AddDays(
            int.Parse(HttpContext.RequestServices.GetService<IConfiguration>()!
                .GetSection("JwtSettings").GetValue<string>("RefreshTokenExpirationDays")!)
        );

        _userService.SaveRefreshToken(user.Id, tokens.RefreshToken, refreshExpiry);

        return Ok(tokens);
    }
}
}

