var builder = WebApplication.CreateBuilder(args);
//add services to DI container

var app = builder.Build();
//add middlewares to proj pipeline

app.Run();
