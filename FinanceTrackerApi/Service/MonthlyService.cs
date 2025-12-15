using FinanceTracker.Models;
using FinanceTrackerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Service
{
    public class MonthlySummaryService
    {
        private readonly ApplicationDbContext _context;

        public MonthlySummaryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlySummary> GenerateMonthlySummary(
            int userId,
            int year,
            int month)
        {
            // 1️⃣ Fetch transactions
            var transactions = await _context.Transactions
                .Where(t =>
                    t.UserId == userId &&
                    t.Datetime2.Year == year &&
                    t.Datetime2.Month == month)
                .ToListAsync();

            // 2️⃣ Calculate totals
     decimal totalIncome = transactions
    .Where(t => t.Type.ToLower() == "income")
    .Sum(t => t.Amount);

decimal totalExpense = transactions
    .Where(t => t.Type.ToLower() == "expense")
    .Sum(t => t.Amount);

            // 3️⃣ Create MonthlySummary entity
            var summary = new MonthlySummary
            {
                UserId = userId,
                Year = year,
                Month = month,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense
            };

            // 4️⃣ Save to DB
            _context.MonthlySummaries.Add(summary);
            await _context.SaveChangesAsync();

            return summary;
        }
    }
}

