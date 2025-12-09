
namespace FinanceTrackerApi.Models
{
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
        public object PasswordHash { get; internal set; }
    }
} 
