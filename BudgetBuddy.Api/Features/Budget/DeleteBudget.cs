namespace BudgetBuddy.Api.Features.Budget;

public class DeleteBudget
{
    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/budget/{month}", (string month) =>
            {
                var budget = BudgetFakeStores.Budgets
                    .FirstOrDefault(b => b.Month == month);

                if (budget == null)
                    return Results.NotFound($"Ingen budget hittade för {month}");

                BudgetFakeStores.Budgets.Remove(budget);
                
                return Results.Ok("Deleted successfully");

            })
            .WithName("DeleteBudget")
            .WithTags("Budget")
            .Produces(StatusCodes.Status404NotFound);
    }

}