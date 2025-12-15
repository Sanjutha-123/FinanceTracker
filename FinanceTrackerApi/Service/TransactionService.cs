using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using static FinanceTracker.Models.Transaction;
using System.Globalization;


namespace FinanceTrackerApi.Data
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------------------- CREATE ----------------------
        public Transaction AddTransaction(Transaction transaction)
        {
            // VALIDATE TYPE → only income or expense allowed
         if (string.IsNullOrWhiteSpace(transaction.Type) || 
    !(transaction.Type.ToLower() == "income" || transaction.Type.ToLower() == "expense"))
{
    throw new ArgumentException("Type must be 'income' or 'expense'");
}


            transaction.Datetime2 = DateTime.Now;

            // If frontend accidentally sends User object
            if (transaction.User != null)
            {
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Email == transaction.User.Email);

                if (existingUser != null)
                {
                    transaction.UserId = existingUser.Id;
                    transaction.User = null;
                }
            }

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        // ---------------------- READ ----------------------
        public List<Transaction> GetByUser(int userId)
        {
            return _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Datetime2)
                .ToList();
        }

        // ---------------------- UPDATE ----------------------
        public bool UpdateTransaction(int id, Transaction updated)
        {
            var existing = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (existing == null)
                return false;

           // VALIDATE TYPE
if (string.IsNullOrWhiteSpace(updated.Type) || 
    !(updated.Type.ToLower() == "income" || updated.Type.ToLower() == "expense"))
{
    throw new ArgumentException("Type must be 'income' or 'expense'");
}


            existing.Amount = updated.Amount;
            existing.Type = updated.Type;
            existing.Category = updated.Category;
            existing.Description = updated.Description;
            existing.Datetime2 = updated.Datetime2;

            _context.SaveChanges();
            return true;
        }

        // ---------------------- DELETE ----------------------
        public bool DeleteTransaction(int id)
        {
            var t = _context.Transactions.FirstOrDefault(x => x.Id == id);
            if (t == null) return false;

            _context.Transactions.Remove(t);
            _context.SaveChanges();
            return true;
        }

        // ---------------------- FILTER ----------------------

    public async Task<List<Transaction>> Filter(
    int userId,
    DateTime? start,
    DateTime? end,
    string? category,
    string? type)
{
    var query = _context.Transactions.AsQueryable();

    query = query.Where(t => t.UserId == userId);

    if (start.HasValue)
        query = query.Where(t => t.Datetime2 >= start.Value);

    if (end.HasValue)
        query = query.Where(t => t.Datetime2 <= end.Value);

    if (!string.IsNullOrEmpty(category))
        query = query.Where(t => t.Category == category);

   if (!string.IsNullOrEmpty(type))
{
    if (type.ToLower() == "income")
        query = query.Where(t => t.Type.ToLower() == "income");
    else if (type.ToLower() == "expense")
        query = query.Where(t => t.Type.ToLower() == "expense");
}


    return await query.ToListAsync();
}
    }
}
