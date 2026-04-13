using YourWallet.Models;

namespace YourWallet.Data.Service
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        Task Add(Expense expense);
        IQueryable GetCharData();
        Task DeleteExpenseAsync(int id);
    }
}
