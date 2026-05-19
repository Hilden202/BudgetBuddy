using BudgetBuddy.Api.Domain.Models;

namespace BudgetBuddy.Api.Features.Auth;


public class AuthFakeStores
{
    public static List<User> Users { get; set; } = new();
}