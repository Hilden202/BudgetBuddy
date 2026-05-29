using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Savings;

public class UpdateSavingsGoal
{
    public record UpdateSavingsGoalRequest(decimal SavingsGoal);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/savings/goal", async (UpdateSavingsGoalRequest request, bbDbContext db, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);

                var dbUser = await db.Users.FindAsync(userId);
                if (dbUser == null)
                    return Results.NotFound();

                dbUser.SavingsGoal = request.SavingsGoal;
                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("UpdateSavingsGoal")
            .WithTags("Savings");
    }
}