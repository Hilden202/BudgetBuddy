namespace BudgetBuddy.Api.Domain;

public class Budget
{
    public Guid  Id { get; set; }

    public Guid userId { get; set; }
    public User User { get; set; }

    public string Month { get; set; }
    public decimal Income { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    //Navigation props
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}