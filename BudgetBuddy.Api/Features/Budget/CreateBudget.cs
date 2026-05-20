using BudgetBuddy.Api.Infrastructure;

namespace BudgetBuddy.Api.Features.Budget;
using Domain.Models;
public static class CreateBudget
{
    public record CreateBudgetRequest(decimal Income, string Month);
    public record CreateBudgetResponse(Guid id, decimal Income, string Month);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/budget", async (CreateBudgetRequest request, bbDbContext db) =>
        {
            var budget = new Budget
            {
                Id = Guid.NewGuid(),
                Month = request.Month,
                Income = request.Income,
                CreatedAt = DateTime.UtcNow,
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            var response = new CreateBudgetResponse(
                budget.Id, 
                budget.Income, 
                budget.Month);

            db.Budgets.Add(budget);
            
            await db.SaveChangesAsync();
            
            return Results.Ok(response);
        })
        .WithName("CreateBudget")
        .WithTags("Budget")
        .Produces<CreateBudgetResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}

