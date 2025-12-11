
using FinanceTracker.Models;
namespace FinanceTrackerApi.Data
{

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // REGISTER USER
   public bool RegisterUser(string username, string email, string hashedPassword)
{
    var existing = _context.Users.FirstOrDefault(u => u.Email == email);
    if (existing != null)
        return false;

    var user = new User
    {
        Username = username,
        Email = email,
        PasswordHash = hashedPassword,
        CreatedAt = DateTime.UtcNow
    };

    _context.Users.Add(user);
    _context.SaveChanges();
    return true;
}
// GET USER BY EMAIL
    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    // SAVE REFRESH TOKEN
    public void SaveRefreshToken(Int32 userId, string refreshToken, DateTime expiry)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return;

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry;

        _context.SaveChanges();
    }

    // VALIDATE CREDENTIALS
    public bool ValidateUserCredentials(string email, string password, out User? user)
    {
        user = GetByEmail(email);
        if (user == null)
            return false;

        bool ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        return ok;
    }

    // REMOVE TOKEN
    public void RevokeRefreshToken(Int32 userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return;

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        _context.SaveChanges();
    }

    // GET USER BY ID
    public User? GetById(Int32 userId)
    {
        return _context.Users.FirstOrDefault(u => u.Id == userId);
    }
    }
}