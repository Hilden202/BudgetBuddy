using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Features.Expenses;

public static class GetExpenses
{
    public record GetResponseExpense(
        Guid Id,
        string Category,
        decimal Amount,
        string? Description
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/expenses/{budgetId}", async (Guid budgetId, bbDbContext db, ClaimsPrincipal user) =>
        {
            var userId = Guid.Parse(user.FindFirstValue("sub")!);
            
            var expenses = await db.Expenses
                .Where(e => e.BudgetId == budgetId
                            && e.Budget.UserId == userId)
                .Select(e => new GetResponseExpense(
                    e.Id,
                    e.Category,
                    e.Amount,
                    e.Description))
                .ToListAsync();

            if (!expenses.Any())
                return Results.NotFound($"Inga utgifter hittades för budget {budgetId}");

            return Results.Ok(expenses);
        })
        .WithName("GetExpenses")
        .WithTags("Expenses")
        .Produces<List<GetResponseExpense>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}