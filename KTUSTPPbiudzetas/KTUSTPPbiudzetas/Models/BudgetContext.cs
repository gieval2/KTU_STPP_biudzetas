using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KTUSTPPBiudzetas.Models
{
    public class BudgetContext : IdentityDbContext<User>
    {
        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        {
            
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
