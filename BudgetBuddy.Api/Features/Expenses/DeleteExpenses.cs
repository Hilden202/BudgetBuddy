using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Expenses;

public class DeleteExpenses
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/expenses/{id:guid}", async (Guid id, bbDbContext db, ClaimsPrincipal user) =>
        {
            var userId = Guid.Parse(user.FindFirstValue("sub")!);
            var expense = await db.Expenses.FindAsync(id);
            
            if (expense == null)
                return Results.NotFound($"Ingen utgift hittades för {id}");
            
            var budget = await db.Budgets.FindAsync(expense.BudgetId);

            if (budget == null)
                return Results.NotFound($"Ingen budget hittades för {expense.BudgetId}");
                
            if (budget.UserId != userId)
                return Results.Unauthorized();
            
            db.Expenses.Remove(expense);
            await db.SaveChangesAsync();
                
            return Results.NoContent();
        })
        .WithName("DeleteExpenses")
        .WithTags("Expenses")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    } 
}
