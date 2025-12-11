namespace FinanceTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }     // FK
        public required User User { get; set; }      // Navigation property

        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;   // Income / Expense
        public string? Category { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}
