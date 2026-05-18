namespace BudgetBuddy.Api.Features.Expenses;
using Domain;

public class ExpensesFakeStores
{
    public static List<Expense> Expenses { get; set; } = new();
}