using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

public class Savings
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string Month { get; set; }
    public User User { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }
    [Precision(18, 2)]
    public decimal GoalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}