using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

public class Budget
{
    public Guid  Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Month { get; set; }
    
    [Precision(18, 2)]
    public decimal Income { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    //Navigation props
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}