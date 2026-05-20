using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

public class Expense
{
    public Guid Id { get; set; }

    public Guid BudgetId { get; set; }
    public string Category { get; set; }
    public Budget Budget { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    
}