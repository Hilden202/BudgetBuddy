using BudgetBuddy.Api.Features.Expenses;

namespace BudgetBuddy.Api.Features.Budget;

public static class GetBudget
{

    public record ExpenseDto(
        Guid Id,
        string Category,
        decimal Amount,
        string? Description
    );
    
    public record Response(
        Guid Id,
        string Month,
        decimal Income,
        List<ExpenseDto> Expenses,   
        decimal remainingAmount
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/budget/{month}", (string month) =>
            {
                var budget = BudgetFakeStores.Budgets
                    .FirstOrDefault(b => b.Month == month);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                var expenses = ExpensesFakeStores.Expenses
                    .Where(e => e.BudgetId == budget.Id)
                    .Select(e => new ExpenseDto(e.Id, e.Category, e.Amount, e.Description))
                    .ToList();

                var remaining = budget.Income - expenses.Sum(e => e.Amount);

                return Results.Ok(new Response(
                    budget.Id,
                    budget.Month,
                    budget.Income,
                    expenses,
                    remaining
                ));
            })
            .WithName("GetBudget")
            .WithTags("budget")
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

    }
}