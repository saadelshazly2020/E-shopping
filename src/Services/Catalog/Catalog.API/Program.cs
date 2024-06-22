var builder = WebApplication.CreateBuilder(args);
//add services to DI container

builder.Services.AddCarter();
builder
    .Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    });
builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
var app = builder.Build();
//add middlewares to proj pipeline
app.MapCarter();
app.Run();