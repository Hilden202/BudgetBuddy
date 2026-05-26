using BudgetBuddy.Api.Infrastructure;
using System.Security.Claims;


namespace BudgetBuddy.Api.Features.Budget;
using Domain.Models;

public static class CreateBudget
{
    public record CreateBudgetRequest(decimal Income, string Month);
    public record CreateBudgetResponse(Guid Id, decimal Income, string Month);

    private static readonly List<(string Name, string Emoji)> defaultCategories = new()
    {
        ("Hyra", "🏠"),
        ("El", "💡"),
        ("Mat", "🍕"),
        ("Transport", "🚗"),
        ("Nöjen", "🎉"),
        ("Sparande", "💎")
    };

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/budget", async (CreateBudgetRequest request, bbDbContext db, ClaimsPrincipal user) =>
        {
            var userId = Guid.Parse(user.FindFirstValue("sub")!);
            var budget = new Budget
            {
                Id = Guid.NewGuid(),
                Month = request.Month,
                Income = request.Income,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            var response = new CreateBudgetResponse(
                budget.Id, 
                budget.Income, 
                budget.Month);

            db.Budgets.Add(budget);

            foreach (var (name, emoji) in defaultCategories)
            {
                db.Expenses.Add(new Expense
                {
                    Id = Guid.NewGuid(),
                    BudgetId = budget.Id,
                    Category = $"{emoji} {name}",
                    Amount = 0,
                    Description = null,
                    CreatedAt = DateTime.UtcNow
                });
            }
            
            await db.SaveChangesAsync();
            
            return Results.Ok(response);
        })
        .WithName("CreateBudget")
        .WithTags("Budget")
        .Produces<CreateBudgetResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}

