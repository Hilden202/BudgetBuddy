using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Budget;

public class DeleteBudget
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/budget/{month}", async (string month, bbDbContext db) =>
            {
                var budget = db.Budgets
                    .FirstOrDefault(b => b.Month == month);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                db.Remove(budget);
                await db.SaveChangesAsync();
                
                return Results.Ok("Deleted successfully");
            })
            .WithName("DeleteBudget")
            .WithTags("Budget")
            .Produces(StatusCodes.Status404NotFound);
    }

}