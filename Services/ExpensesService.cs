using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using YourWallet.Data;
using YourWallet.Models;

namespace YourWallet.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly YourWalletContext _context;

        public ExpensesService(YourWalletContext context)
        {
            _context = context;
        }

        public async Task Add(Expense expense, int userId)
        {
            expense.AppUserId = userId;
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Expense>> GetAll(int userId)
        {
            return await _context.Expenses
                .Where(e => e.AppUserId == userId)
                .ToListAsync();
        }

        public IQueryable GetCharData(int userId)
        {
            return _context.Expenses
                .Where(e => e.AppUserId == userId)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount)
                });
        }

        public async Task DeleteExpenseAsync(int id, int userId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.AppUserId == userId);
        }
    }
}