using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;
using BudgetBuddy.Api.Domain.Models;

namespace BudgetBuddy.Api.Features.Savings;

public class CreateSavings
{
    public record CreateSavingsRequest(
        Guid userId,
        string Month,
        decimal Amount
    );

    public record CreateSavingsResponse(
        Guid id,
        string Month,
        decimal Amount
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/savings", async (CreateSavingsRequest request, bbDbContext db, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);
                var savings = new Saving
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Month = request.Month,
                    Amount = request.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                db.Savings.Add(savings);
                await db.SaveChangesAsync();

                var response = new CreateSavingsRequest(
                    savings.Id,
                    savings.Month,
                    savings.Amount);

                return Results.Created($"api/savings/{savings.Id}",
                    response);
            })
            .WithName("CreateSavings")
            .WithTags("Savings")
            .Produces<CreateSavingsResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

    }
}