

var builder = WebApplication.CreateBuilder(args);

//ADD DI services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);


var app = builder.Build();

//use services in application pipeline
app.UseApiServices();
//migrate database 

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.Run();
