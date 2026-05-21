using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetBuddy.Api.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BudgetBuddy.Api.Features.Auth;

public class Login
{
    public record LoginRequest(string Email, string Password);
    public record LoginResponse(string Token);

    public static void MapEndPoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async (
                LoginRequest request,
                UserManager<User> userManager,
                IConfiguration config) =>
            {
                //1. Hitta användaren
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Results.Unauthorized();

                //2. Kolla Lösenordet
                var validPassword = await userManager.CheckPasswordAsync(user, request.Password);
                if (!validPassword)
                    return Results.Unauthorized();

                //3. Skapa Token
                var token = CreateToken(user, config);
                return Results.Ok(new LoginResponse(token));
            })
            .WithName("Login")
            .WithTags("Auth");
    }

    private static string CreateToken(User user, IConfiguration config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        
    }
}