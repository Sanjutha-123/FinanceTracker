using BCrypt.Net;
using FinanceTrackerApi.Models;
using FinanceTrackerApi.Data;
namespace FinanceTrackerApi.Service
{

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool RegisterUser(string username, string email, string password)
    {
        // Check if the user already exists
        if (_context.Users.Any(u => u.Username == username || u.Email == email))
        {
            return false; // User already exists
        }

        // Hash the password using bcrypt
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Create the user
        var user = new User
        {
            Username = username,
            Email = email,
            HashedPassword = hashedPassword
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return true; // Registration successful
    }
}
}
