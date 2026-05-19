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
        app.MapGet("/api/expenses/{budgetId}", (Guid budgetId) =>
        {
            var expenses = ExpensesFakeStores.Expenses
                .Where(e => e.BudgetId == budgetId)
                .Select(e => new GetResponseExpense(
                    e.Id,
                    e.Category,
                    e.Amount,
                    e.Description))
                .ToList();

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