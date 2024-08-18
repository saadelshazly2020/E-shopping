
var builder = WebApplication.CreateBuilder(args);

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



//Add Grpc client to 
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);

}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

//async communication [add message broker to services DI]
builder.Services.AddMessageBroker(builder.Configuration);

//crosscutting concerns
//1. exception handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
//2. add health check service for application and postresql database
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("redis")!);//add health check service for app and postgresql database

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
