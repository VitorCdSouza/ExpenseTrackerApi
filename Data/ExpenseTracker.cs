using Microsoft.EntityFrameworkCore;

public class ExpenseTrackerContext : DbContext
{
    public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Report> Reports { get; set; }
}
