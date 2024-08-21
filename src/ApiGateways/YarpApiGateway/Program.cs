var builder = WebApplication.CreateBuilder(args);

//add services to DI

var app = builder.Build();

//add http request pipelines

app.Run();
