using BudgetBuddy.Api.Domain.Models;

namespace BudgetBuddy.Api.Features.Expenses;

public class 
    ExpensesFakeStores
{
    public static List<Expense> Expenses { get; set; } = new();
}