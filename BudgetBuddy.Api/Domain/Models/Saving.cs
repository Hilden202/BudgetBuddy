using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

public class Saving
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string Month { get; set; } = string.Empty;
    public User User { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; } 
    public DateTime CreatedAt { get; set; }
}