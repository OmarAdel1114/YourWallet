using YourWallet.Models;

namespace YourWallet.Services
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll(int userId);
        Task Add(Expense expense, int userId);
        IQueryable GetCharData(int userId);
        Task DeleteExpenseAsync(int id, int userId);
    }
}