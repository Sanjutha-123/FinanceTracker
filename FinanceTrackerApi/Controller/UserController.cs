using FinanceTracker.Models;
using FinanceTrackerApi.Service;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;  // Import BCrypt.Net

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
        // Validate request data
        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest("Invalid data.");
        }

        // Hash the password using bcrypt before passing it to the service
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Now call the service method to register the user with the hashed password
        bool result = _userService.RegisterUser(request.Username, request.Email, hashedPassword);

        if (!result)
        {
            return Conflict("User already exists.");
        }

        return Ok("Registration successful.");
    }
}



