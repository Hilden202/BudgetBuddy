using BudgetBuddy.Api.Domain.Models;
using BudgetBuddy.Api.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Api.Features.Auth;

public class Register
{
    public record RegisterRequest(string Email, string Password);
    public record RegisterResponse(string UserId, string Email);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (RegisterRequest request, UserManager<User> userManager) =>
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };
            
            var result = await userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);
            
            return Results.Ok($"Användare skapad!");
        })
        .WithName("Register")
        .WithTags("Auth")
        .Produces<RegisterResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}