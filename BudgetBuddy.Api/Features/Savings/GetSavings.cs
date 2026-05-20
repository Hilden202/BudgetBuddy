using BudgetBuddy.Api.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Features.Savings;
using Domain.Models;

public class GetSavings
{
    public record GetSavingsResponse(
        Guid Id,
        string Month,
        decimal Amount,
        decimal GoalAmount
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/savings/{userId:guid}", async (Guid userId, bbDbContext db) =>
            {
                var savings = await db.Savings
                    .Where(s => s.UserId == userId)
                    .Select(s => new GetSavingsResponse(
                        s.Id,
                        s.Month,
                        s.Amount,
                        s.GoalAmount))
                    .ToListAsync();

                if (!savings.Any())
                    return Results.NotFound($"Inga sparanden hittades för användaren {userId}");

                return Results.Ok(savings);
            })
            .WithName("GetSavings")
            .WithTags("Savings")
            .Produces<List<GetSavingsResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

