using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Expenses;

public class UpdateExpenses
{
    public record UpdateExpensesRequest(decimal Amount, string Category, string Description);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/expenses/{id:guid}", async (Guid id, UpdateExpensesRequest request, bbDbContext db) =>
        {
            var expense = await db.Expenses.FindAsync(id);

            if (expense == null)
                return Results.NotFound($"Inga utgifter hittades med id {id}");
            
            expense.Amount = request.Amount;
            expense.Category = request.Category;
            expense.Description = request.Description;
            
            await db.SaveChangesAsync();
            return Results.NoContent();
            
        })
        .WithName("UpdateExpenses")
        .WithTags("Expenses");
    }
}