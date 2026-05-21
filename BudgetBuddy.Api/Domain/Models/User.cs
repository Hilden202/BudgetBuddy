using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;
    
    // Navigation props
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<Savings> Savings { get; set; } = new List<Savings>();

}