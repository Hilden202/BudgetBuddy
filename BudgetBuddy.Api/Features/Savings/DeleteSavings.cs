using BudgetBuddy.Api.Infrastructure;
using System.Security.Claims;

namespace BudgetBuddy.Api.Features.Savings;

public class DeleteSavings
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/savings{id:guid}", async (Guid id, bbDbContext db, ClaimsPrincipal user) =>
        {
            var userId = Guid.Parse(user.FindFirstValue("sub")!);
            var savings = await db.Savings.FindAsync(id);

            if (savings == null)
                return Results.NotFound($"Inget sparande hittades med id {id}");

            if (savings.UserId != userId)
                return Results.Unauthorized();

            db.Savings.Remove(savings);
            await db.SaveChangesAsync();
            
            return Results.Ok("Deleted successfully");
        })
        .WithName("DeleteSavings")
        .WithTags("Savings");
    }
}