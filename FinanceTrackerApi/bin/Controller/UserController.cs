using FinanceTracker.Models;
using FinanceTrackerApi.Service;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // POST: api/Users/Register
    [HttpPost("Register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest("Invalid data.");
        }

        bool result = _userService.RegisterUser(request.Username, request.Email, request.Password);

        if (!result)
        {
            return Conflict("User already exists.");
        }

        return Ok("Registration successful.");
    }
}
