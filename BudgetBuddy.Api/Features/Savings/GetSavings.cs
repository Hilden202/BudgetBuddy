using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Features.Savings;

public class GetSavings
{
    public record GetSavingsResponse(
        decimal MonthAmount,
        decimal TotalAmount,
        decimal SavingsGoal
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/savings/{month}", async (string month, bbDbContext db, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);

                var monthSavings = await db.Savings
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.Month == month);

                var totalAmount = await db.Savings
                    .Where(s => s.UserId == userId)
                    .SumAsync(s => s.Amount);

                var savingsGoal = await db.Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.SavingsGoal)
                    .FirstOrDefaultAsync();

                return Results.Ok(new GetSavingsResponse(
                    monthSavings?.Amount ?? 0,
                    totalAmount,
                    savingsGoal
                ));
            })
            .WithName("GetSavings")
            .WithTags("Savings")
            .Produces<GetSavingsResponse>(StatusCodes.Status200OK);
    }
}