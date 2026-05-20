using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Api.Domain.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    
    // public string PasswordHash { get; set; }
    public DateTime CreatedAt  { get; set; }
    
    // Navigation props
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<Savings> Savings { get; set; } = new List<Savings>();

}