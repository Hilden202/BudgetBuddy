using BudgetBuddy.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Api.Infrastructure;

public class bbDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public bbDbContext(DbContextOptions<bbDbContext> options)
        : base(options)
    {
    }
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Savings> Savings => Set<Savings>();
    
}