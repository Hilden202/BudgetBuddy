using BudgetBuddy.Api.Domain.Models;

namespace BudgetBuddy.Api.Infrastructure.Seed;

public class SeedData
{
    public static void Initialize(bbDbContext dbContext)
    {
        if (!dbContext.Users.Any())
        {
            dbContext.Users.Add(new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "dev@budgetbuddy.local",
            });
        
            dbContext.SaveChanges();
        }
    }
}