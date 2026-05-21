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
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
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

//budget endpoints
CreateBudget.MapEndPoint(app);
GetBudget.MapEndPoint(app);
UpdateBudget.MapEndPoint(app);
DeleteBudget.MapEndPoint(app);

//Expense Endpoints
CreateExpenses.MapEndPoint(app);
GetAllExpenses.MapEndPoint(app);
GetExpenses.MapEndPoint(app);
DeleteExpenses.MapEndPoint(app);
UpdateExpenses.MapEndPoint(app);

//savings Endpoints
CreateSavings.MapEndPoint(app);
GetSavings.MapEndPoint(app);
DeleteSavings.MapEndPoint(app);
UpdateSavings.MapEndPoint(app);
GetTotalSavings.MapEndPoint(app);

//Auth endpoints
Register.MapEndPoint(app);
Login.MapEndPoint(app);

app.Run();
