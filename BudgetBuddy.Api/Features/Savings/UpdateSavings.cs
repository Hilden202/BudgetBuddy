using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Savings;

public class UpdateSavings
{
    public record UpdateSavingsRequest(decimal Amount, decimal GoalAmount);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/savings/{id:guid}", async (Guid id, UpdateSavingsRequest request, bbDbContext db) =>
            {
                var savings = await db.Savings.FindAsync(id);

                if (savings == null)
                    return Results.NotFound($"Inget sparande hittades med id {id}");

                savings.Amount = request.Amount;
                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("UpdateSavings")
            .WithTags("Savings");
    }
    
    
}