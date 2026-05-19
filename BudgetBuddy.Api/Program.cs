using BudgetBuddy.Api.Features.Budget;
using BudgetBuddy.Api.Features.Expenses;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);
//swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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


app.Run();
