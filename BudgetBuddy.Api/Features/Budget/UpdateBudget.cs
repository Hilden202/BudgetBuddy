using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Api.Features.Budget;


public class UpdateBudget
{
    public record UpdateBudgetRequest(decimal Income);
    public record UpdateBudgetResponse(Guid Id,  string Month, decimal Income);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/budget/{month}", (string month, [FromBody] UpdateBudgetRequest request) =>
            {
                var budget = BudgetFakeStores.Budgets
                    .FirstOrDefault(b => b.Month == month);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                budget.Income = request.Income;

                var response = new UpdateBudgetResponse(
                    budget.Id, 
                    budget.Month, 
                    budget.Income);
                return Results.Ok(response);

            })
            .WithName("UpdateBudget")
            .WithTags("Budget")
            .Produces<UpdateBudgetResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}