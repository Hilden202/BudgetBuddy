using BudgetBuddy.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Infrastructure;

public class bbDbContext : DbContext
{
    public bbDbContext(DbContextOptions<bbDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Savings> Savings => Set<Savings>();
    
}