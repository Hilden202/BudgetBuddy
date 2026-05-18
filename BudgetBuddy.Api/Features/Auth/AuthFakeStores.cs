namespace BudgetBuddy.Api.Features.Auth;
using Domain;

public class AuthFakeStores
{
    public static List<User> Users { get; set; } = new();
}