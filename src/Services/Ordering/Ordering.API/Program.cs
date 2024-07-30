using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//ADD DI services
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();


var app = builder.Build();

//use services in application pipeline
app.UseApiServices();
//migrate database 

if (app.Environment.IsDevelopment())
{
   await app.InitializeDatabaseAsync();
}

app.Run();
