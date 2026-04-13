using Microsoft.EntityFrameworkCore;
using YourWallet.Models;

namespace YourWallet.Data
{
    public class YourWalletContext : DbContext
    {
        public YourWalletContext(DbContextOptions<YourWalletContext> options) : base(options)
        {
        } 

       public DbSet<Expense> Expenses { get; set; }
    }
}
