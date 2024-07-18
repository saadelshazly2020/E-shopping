

using Basket.API.Data;
using BuildingBlocks.Exceptions.handlers;
using Marten;

var builder = WebApplication.CreateBuilder(args);
//configure services here 


//add services to DI container

builder.Services.AddCarter();//minimal api
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);//register mediator service from this assembly
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));//add validation behavior to mediator pipeline 
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));//add logging behavior to mediator pipeline 
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);//add all fluent validation validators
builder.Services.AddMarten(config =>//for postgresql ORM
{
    config.Connection(builder.Configuration.GetConnectionString("Database")!);
    config.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();


builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//configure app pipeline here

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
