using Microsoft.EntityFrameworkCore;
using YourWallet.Models;
using YourWallet.Models.Domain;

namespace YourWallet.Data
{
    public class YourWalletContext : DbContext
    {
        public YourWalletContext(DbContextOptions<YourWalletContext> options) : base(options) { } 
       public DbSet<Expense> Expenses { get; set; }
       public DbSet<AppUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.GoogleId).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.GoogleId).IsRequired();
            });
        }

    }
}
