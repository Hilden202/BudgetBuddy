using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;
using BudgetBuddy.Api.Domain.Models;

namespace BudgetBuddy.Api.Features.Expenses;



public class UpdateExpenses
{
    public record UpdateExpensesRequest(decimal Amount, string Category, string Description);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/expenses/{id:guid}", async (Guid id, UpdateExpensesRequest request, bbDbContext db, ClaimsPrincipal user) =>
        {
            var expense = await db.Expenses.FindAsync(id);
            if (expense == null)
                return Results.NotFound($"Inga utgifter hittades med id {id}");
            
            expense.Amount = request.Amount;
            expense.Category = request.Category;
            expense.Description = request.Description;

            // om kategorin är Sparande -> Synka till savings-tabellen
            if (request.Category.Contains("Sparande"))
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);
                
                //hämta månad från budget
                var budget = await db.Budgets.FindAsync(expense.BudgetId);
                var month = budget!.Month;
                
                //Kollar om det finns en savings-post för denna månad
                var existing = db.Savings
                    .FirstOrDefault(s => s.UserId == userId && s.Month == month);

                if (existing != null)
                {
                    existing.Amount = request.Amount;
                }
                else
                {
                    db.Savings.Add(new Domain.Models.Savings
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Month = month,
                        Amount = request.Amount,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                
            }
            
            await db.SaveChangesAsync();
            return Results.NoContent();
            
        })
        .WithName("UpdateExpenses")
        .WithTags("Expenses");
    }
}