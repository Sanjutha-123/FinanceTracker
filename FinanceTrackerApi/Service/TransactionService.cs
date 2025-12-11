using FinanceTracker.Models;

namespace FinanceTrackerApi.Data
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        public Transaction AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        // READ (All by user)
        public List<Transaction> GetByUser(int userId)
        {
            return _context.Transactions
                .Where(t => t.UserId == userId)
                .ToList();
        }

        // UPDATE
        public bool UpdateTransaction(Transaction updated)
        {
            var existing = _context.Transactions.FirstOrDefault(t => t.Id == updated.Id);
            if (existing == null) return false;

            existing.Amount = updated.Amount;
            existing.Type = updated.Type;
            existing.Category = updated.Category;
            existing.Description = updated.Description;
            existing.Date = updated.Date;

            _context.SaveChanges();
            return true;
        }

        // DELETE
        public bool DeleteTransaction(int id)
        {
            var t = _context.Transactions.FirstOrDefault(x => x.Id == id);
            if (t == null) return false;

            _context.Transactions.Remove(t);
            _context.SaveChanges();
            return true;
        }

        internal object AddTransaction(System.Transactions.Transaction request)
        {
            throw new NotImplementedException();
        }
    }
}
