using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Expenses;

public class DeleteExpenses
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/expenses/{id:guid}", async (Guid id, bbDbContext db) =>
        {
            var expense = await db.Expenses.FindAsync(id);
            
            if (expense == null)
                return Results.NotFound($"Ingen budget hittade för {id}");
            
            db.Expenses.Remove(expense);
            await db.SaveChangesAsync();
                
            return Results.Ok("Deleted successfully");
        })
        .WithName("DeleteExpenses")
        .WithTags("Expenses")
        .Produces(StatusCodes.Status404NotFound);
    } 
}
