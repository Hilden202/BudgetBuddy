using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Savings;
using Domain.Models;

public class CreateSavings
{
    public record CreateSavingsRequest(
        Guid userId,
        string Month,
        decimal Amount,
        decimal GoalAmount
    );

    public record CreateSavingsResponse(
        Guid id,
        string Month,
        decimal Amount,
        decimal GoalAmount
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/savings", async (CreateSavingsRequest request, bbDbContext db) =>
            {
                var savings = new Savings
                {
                    Id = Guid.NewGuid(),
                    UserId = request.userId,
                    Month = request.Month,
                    Amount = request.Amount,
                    GoalAmount = request.GoalAmount,
                    CreatedAt = DateTime.UtcNow
                };

                db.Savings.Add(savings);
                await db.SaveChangesAsync();

                var response = new CreateSavingsRequest(
                    savings.Id,
                    savings.Month,
                    savings.Amount,
                    savings.GoalAmount);

                return Results.Created($"api/savings/{savings.Id}",
                    response);
            })
            .WithName("CreateSavings")
            .WithTags("Savings")
            .Produces<CreateSavingsResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

    }
}