using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Savings;


public class DeleteSavings
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/savings{id:guid}", async (Guid id, bbDbContext db) =>
        {
            var savings = await db.Savings.FindAsync(id);

            if (savings == null)
                return Results.NotFound($"Inget sparande hittades med id {id}");

            db.Savings.Remove(savings);
            await db.SaveChangesAsync();
            
            return Results.Ok("Deleted successfully");
        })
        .WithName("DeleteSavings")
        .WithTags("Savings");
    }
}