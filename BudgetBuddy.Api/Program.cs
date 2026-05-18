using BudgetBuddy.Api.Features.Budget;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);
//swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//swaggerMiddleWare
app.UseSwagger();
app.UseSwaggerUI();


//test EndPoint
app.MapGet("/", () => Results.Redirect("/swagger"));

//budget endpoints
CreateBudget.MapEndpoint(app);
GetBudget.MapEndPoint(app);

app.Run();
