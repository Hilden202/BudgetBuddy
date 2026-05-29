using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Api.Features.Budgets;


public class UpdateBudget
{
    public record UpdateBudgetRequest(decimal Income);
    public record UpdateBudgetResponse(Guid Id,  string Month, decimal Income);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/budget/{month}",async (string month, [FromBody] UpdateBudgetRequest request, bbDbContext db, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);
                
                var budget = db.Budgets
                    .FirstOrDefault(b => b.Month == month
                                         && b.UserId == userId);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                budget.Income = request.Income;

                var response = new UpdateBudgetResponse(
                    budget.Id, 
                    budget.Month, 
                    budget.Income);
                
                db.Budgets.Update(budget);
                await db.SaveChangesAsync();
                
                return Results.Ok(response);

            })
            .WithName("UpdateBudget")
            .WithTags("Budget")
            .Produces<UpdateBudgetResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}