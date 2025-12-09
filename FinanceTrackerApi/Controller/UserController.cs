using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using FinanceTrackerApi.Models;
namespace UserRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        // Constructor to inject the DbContext
        public UserController(UserDbContext context)
        {
            _context = context;
        }

        // POST: api/User/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return BadRequest("Invalid data.");
            }

            // Check if a user already exists with the same email or username
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerModel.Email || u.Username == registerModel.Username);

            if (existingUser != null)
            {
                return BadRequest("A user with this email or username already exists.");
            }

            // Create a new user object
            var user = new User
            {
                Email = registerModel.Email,
                Username = registerModel.Username,
                PasswordHash = HashPassword(registerModel.Password)
            };

            // Save the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        // Private method to hash the password securely
        private string HashPassword(string password)
        {
            // Generate a salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt using PBKDF2
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }
    }

    public class RegisterModel
    {
    }

    internal class UserDbContext
    {
        public object Users { get; internal set; }

        internal async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

  



