using BudgetBuddy.Api.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Api.Infrastructure.Seed;

public class SeedData
{
    public static async Task Initialize(UserManager<User> userManager)
    {
        var email = "dev@budgetbuddy.local";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            await userManager.CreateAsync(user, "Dev1234!");
        }
    }
}