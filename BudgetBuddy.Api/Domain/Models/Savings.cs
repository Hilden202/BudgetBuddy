namespace BudgetBuddy.Api.Domain;

public class Savings
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string Month { get; set; }
    public User User { get; set; }

    public decimal Amount { get; set; }
    public decimal GoalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}