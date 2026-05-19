namespace BudgetBuddy.Api.Features.Budget;
using Domain;

public static class CreateBudget
{
    public record CreateBudgetRequest(decimal Income, string Month);
    public record CreateBudgetResponse(Guid id, decimal Income, string Month);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/budget", async (CreateBudgetRequest request) =>
        {
            var budget = new Domain.Models.Budget
            {
                Id = Guid.NewGuid(),
                Month = request.Month,
                Income = request.Income,
            };

            BudgetFakeStores.Budgets.Add(budget);

            var response = new CreateBudgetResponse(
                budget.Id, 
                budget.Income, 
                budget.Month);

            return Results.Ok(response);
        })
        .WithName("CreateBudget")
        .WithTags("Budget")
        .Produces<CreateBudgetResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}

