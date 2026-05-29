using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Budgets;

public class DeleteBudget
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/budget/{month}", async (string month, bbDbContext db, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);
                var budget = db.Budgets
                    .FirstOrDefault(b => b.Month == month
                                         && b.UserId == userId);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                db.Budgets.Remove(budget);
                await db.SaveChangesAsync();

                return Results.Ok("Deleted successfully");
            })
            .WithName("DeleteBudget")
            .WithTags("Budget")
            .Produces(StatusCodes.Status404NotFound);
    }

}