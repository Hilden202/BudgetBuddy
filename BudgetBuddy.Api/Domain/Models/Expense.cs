namespace BudgetBuddy.Api.Domain.Models;

public class Expense
{
    public Guid Id { get; set; }

    public Guid BudgetId { get; set; }
    public string Category { get; set; }
    public Budget Budget { get; set; }

    public decimal Amount { get; set; }
    public string Description { get; set; }
    
}