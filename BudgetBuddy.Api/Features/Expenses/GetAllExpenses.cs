
using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Expenses;

public class GetAllExpenses
{
    public record GetAllExpensesResponse(
        Guid Id,
        string Category,
        decimal Amount,
        string? Description,
        DateTime CreatedAt
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/expenses", async (bbDbContext db) =>
            {
                var expenses = db.Expenses
                    .Select(e => new GetAllExpensesResponse(
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
            .Produces<List<GetAllExpensesResponse>>(StatusCodes.Status200OK);
    }
}