using System.Text;
using BudgetBuddy.Api.Domain.Models;
using BudgetBuddy.Api.Features.Auth;
using BudgetBuddy.Api.Features.Budget;
using BudgetBuddy.Api.Features.Expenses;
using BudgetBuddy.Api.Features.Savings;
using BudgetBuddy.Api.Infrastructure;
using BudgetBuddy.Api.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity; // Ta inte bort
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
//swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cores
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// EF Core
builder.Services.AddDbContext<bbDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

//Idententity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.ClaimsIdentity.UserIdClaimType = "sub";
    })
    .AddEntityFrameworkStores<bbDbContext>()
    .AddDefaultTokenProviders();

//Jwt-autentisering
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>(); 
    await SeedData.Initialize(userManager);
}

//swaggerMiddleWare
app.UseSwagger();
app.UseSwaggerUI();


//Swagger EndPoint
app.MapGet("/", () => Results.Redirect("/swagger")) 
    .ExcludeFromDescription();

//auth för endpoints
var api = app.MapGroup("").RequireAuthorization();

//budget endpoints
CreateBudget.MapEndPoint(api);
GetBudget.MapEndPoint(api);
UpdateBudget.MapEndPoint(api);
DeleteBudget.MapEndPoint(api);

//Expense Endpoints
CreateExpenses.MapEndPoint(api);
GetAllExpenses.MapEndPoint(api);
GetExpenses.MapEndPoint(api);
DeleteExpenses.MapEndPoint(api);
UpdateExpenses.MapEndPoint(api);

//savings Endpoints
CreateSavings.MapEndPoint(api);
GetSavings.MapEndPoint(api);
DeleteSavings.MapEndPoint(api);
UpdateSavings.MapEndPoint(api);
GetTotalSavings.MapEndPoint(api);
UpdateSavingsGoal.MapEndPoint(api);

//Auth endpoints
Register.MapEndPoint(app);
Login.MapEndPoint(app);

app.Run();
