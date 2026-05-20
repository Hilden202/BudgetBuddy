using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Expenses;
using Domain.Models;

public class CreateExpenses
{
    public record CreateExpenseRequest(
        Guid BudgetId,
        string Category,
        decimal Amount,
        string Description
    );
    
    public record CreateExpenseResponse(
        Guid Id,
        string Category,
        decimal Amount,
        string Description
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/expenses", async (CreateExpenseRequest request, bbDbContext db) =>
            {
                var expenses = new Expense()
                {
                    Id = Guid.NewGuid(),
                    BudgetId = request.BudgetId,
                    Category = request.Category,
                    Amount = request.Amount,
                    Description = request.Description
                };

                var response = new CreateExpenseRequest(
                    expenses.Id,
                    expenses.Category,
                    expenses.Amount,
                    expenses.Description);
                
                db.Expenses.Add(expenses);
                await db.SaveChangesAsync();

                return Results.Ok(response);
            })
            .WithName("CreateExpenses")
            .WithTags("Expenses")
            .Produces<CreateExpenseResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}