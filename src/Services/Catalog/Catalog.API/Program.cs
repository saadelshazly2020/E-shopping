
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
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())//seed data id development
    builder.Services.InitializeMartenWith<CatalogInitializeData>();


builder.Services.AddExceptionHandler<CustomExceptionHandler>();//add custom exception handler service 
//add health check service for application and postresql database

builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);//add health check service for app and postgresql database

var app = builder.Build();

//add middlewares to proj pipeline


app.MapCarter();

app.UseExceptionHandler(opt => { });

//use ui option for health check to be in json format response
app.UseHealthChecks("/Health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
