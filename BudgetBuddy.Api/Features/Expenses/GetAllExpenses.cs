
using System.Security.Claims;
using BudgetBuddy.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Features.Expenses;

public class GetAllExpenses
{
    public record GetAllExpensesResponse(
        Guid Id,
        string Category,
        decimal Amount,
        string? Description,
        DateTime CreatedAt
    );

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/expenses", async (bbDbContext db, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirstValue("sub")!);
                
                var expenses = await db.Expenses
                    .Where(e => e.Budget.UserId == userId)
                    .Select(e => new GetAllExpensesResponse(
                        e.Id,
                        e.Category,
                        e.Amount,
                        e.Description,
                        e.CreatedAt))
                    .ToListAsync();
                
                return Results.Ok(expenses);
            })
            .WithName("GetAllExpenses")
            .WithTags("Expenses")
            .Produces<List<GetAllExpensesResponse>>(StatusCodes.Status200OK);
    }
}