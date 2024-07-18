

using Basket.API.Data;
using BuildingBlocks.Exceptions.handlers;
using Marten;
using Microsoft.Extensions.Caching.Distributed;

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



builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//add caching CachedBasketRepository decorator[decorator pattern]
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//you can register CachedBasketRepository decorator manually like the following

//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository=provider.GetRequiredService<IBasketRepository>();
//    var distributedCache= provider.GetRequiredService<IDistributedCache>();
//    return new CachedBasketRepository(basketRepository, distributedCache);
//});


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("redis");
    //options.InstanceName = "SampleInstance";
});


var app = builder.Build();

//configure app pipeline here

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
