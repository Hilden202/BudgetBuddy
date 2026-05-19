
namespace BudgetBuddy.Api.Features.Expenses;

public class GetAllExpenses
{
    public record Response(
        Guid Id,
        string Category,
        decimal Amount,
        string? Description,
        DateTime CreatedAt
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/expenses", () =>
            {
                var expenses = ExpensesFakeStores.Expenses
                    .Select(e => new Response(
                        e.Id,
                        e.Category,
                        e.Amount,
                        e.Description,
                        e.CreatedAt))
                    .ToList();

                return Results.Ok(expenses);
            })
            .WithName("GetAllExpenses")
            .WithTags("Expenses")
            .Produces<List<Response>>(StatusCodes.Status200OK);
    }
}