using System.Text;
using BudgetBuddy.Api.Domain.Models;
using BudgetBuddy.Api.Features.Auth;
using BudgetBuddy.Api.Features.Budgets;
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

// Swagger JWT-konfiguration för att kunna skicka Bearer-token via Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Skriv: Bearer {din token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
