namespace BudgetBuddy.Api.Features.Budget;
using Domain;

public static class CreateBudget
{
    public record Request(decimal Income, string Month);
    public record Response(Guid id, decimal Income, string Month);

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/budget", async (Request request) =>
        {
            decimal remainingAmount = request.Income;

            var budget = new Budget
            {
                Id = Guid.NewGuid(),
                Month = request.Month,
                Income = request.Income,
            };

            BudgetFakeStores.Budgets.Add(budget);

            var response = new Response(budget.Id, budget.Income, budget.Month);

            return Results.Ok(response);
        });
    }
}

