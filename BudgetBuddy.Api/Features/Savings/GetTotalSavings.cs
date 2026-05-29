using BudgetBuddy.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Features.Savings;

public class GetTotalSavings
{
    public record GetTotalSavingsResponse(
        decimal TotalAmount
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/savings/{userId:guid}/total", async (Guid userId, bbDbContext db) =>
            {
                var total = await db.Savings
                    .Where(s => s.UserId == userId)
                    .SumAsync(s => s.Amount);
                
                return Results.Ok(new GetTotalSavingsResponse(total));
            })
            .WithName("GetTotalSavings")
            .WithTags("Savings")
            .Produces<GetTotalSavingsResponse>(StatusCodes.Status200OK);
    }
}