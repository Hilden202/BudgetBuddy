using BudgetBuddy.Api.Features.Expenses;
using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Budget;

public static class GetBudget
{

    public record ExpenseResponse(
        Guid Id,
        string Category,
        decimal Amount,
        string? Description
    );
    
    public record GetBudgetResponse(
        Guid Id,
        string Month,
        decimal Income,
        List<ExpenseResponse> Expenses
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/budget/{month}", async (string month, bbDbContext db) =>
            {
                var budget =  db.Budgets
                    .FirstOrDefault(b => b.Month == month);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                var expenses = ExpensesFakeStores.Expenses
                    .Where(e => e.BudgetId == budget.Id)
                    .Select(e => new ExpenseResponse(e.Id, e.Category, e.Amount, e.Description))
                    .ToList();
                
                return Results.Ok(new GetBudgetResponse(
                    budget.Id,
                    budget.Month,
                    budget.Income,
                    expenses
                ));
            })
            .WithName("GetBudget")
            .WithTags("Budget")
            .Produces<GetBudgetResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

    }
}