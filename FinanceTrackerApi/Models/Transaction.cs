namespace FinanceTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }     // Foreign key
        public User? User { get; set; }     // Navigation property

        public decimal Amount { get; set; }

        public enum TransactionType
        {
            income,
            expense
        }

        public TransactionType Type { get; set; }   // Allowed ONLY: income/expense

        public string? Category { get; set; }
        public string? Description { get; set; }

        public DateTime Datetime2 { get; set; } = DateTime.UtcNow;
    }
}

