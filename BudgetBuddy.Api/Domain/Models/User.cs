using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;
    
    [Precision(18, 2)]
    public decimal SavingsGoal { get; set; } = 0;
    
    // Navigation props
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<Saving> Savings { get; set; } = new List<Saving>();

}