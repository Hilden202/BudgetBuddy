using BudgetBuddy.Api.Domain.Models;
using BudgetBuddy.Api.Features.Auth;
using BudgetBuddy.Api.Features.Budget;
using BudgetBuddy.Api.Features.Expenses;
using BudgetBuddy.Api.Features.Savings;
using BudgetBuddy.Api.Infrastructure;
using BudgetBuddy.Api.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity; // Ta inte bort
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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


var app = builder.Build();


// Automatic migration when starting project
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<bbDbContext>();
//     dbContext.Database.Migrate();
//     
//     //SeedUser 
//     //SeedData.Initialize(dbContext);
// }

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

//Register
Register.MapEndPoint(app);

app.Run();
