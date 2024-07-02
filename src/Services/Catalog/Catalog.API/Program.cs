


using Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);
//add services to DI container

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitializeData>();


builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();


//add middlewares to proj pipeline
app.MapCarter();

app.UseExceptionHandler(opt => { });
app.Run();
