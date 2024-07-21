

using Basket.API.Data;
using BuildingBlocks.Exceptions.handlers;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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


//add health check service for application and postresql database
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("redis")!);//add health check service for app and postgresql database
//Add Grpc client to 
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);

}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler { 
    ServerCertificateCustomValidationCallback=HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});


var app = builder.Build();

//configure app pipeline here

app.MapCarter();

app.UseExceptionHandler(options => { });

//use ui option for health check to be in json format response
app.UseHealthChecks("/Health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
