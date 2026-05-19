namespace BudgetBuddy.Api.Features.Expenses;

public class CreateExpenses
{
    public record CreateExpenseRequest(
        Guid BudgetId,
        string Category,
        decimal Amount,
        string Description
    );
    
    public record CreateExpenseResponse(
        Guid Id,
        string Category,
        decimal Amount,
        string Description
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/expenses", async (CreateExpenseRequest request) =>
            {
                var expenses = new Domain.Models.Expense
                {
                    Id = Guid.NewGuid(),
                    BudgetId = request.BudgetId,
                    Category = request.Category,
                    Amount = request.Amount,
                    Description = request.Description
                };

                ExpensesFakeStores.Expenses.Add(expenses);

                var response = new CreateExpenseRequest(
                    expenses.Id,
                    expenses.Category,
                    expenses.Amount,
                    expenses.Description);

                return Results.Ok(response);
            })
            .WithName("CreateExpenses")
            .WithTags("Expenses")
            .Produces<CreateExpenseResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}