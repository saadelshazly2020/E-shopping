
namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            //add interceptors to DI
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            //add db context to services with connection string configuration
            services
                .AddDbContext<ApplicationDBContext>((sp, opt) =>
                {
                    //add all interceptors from DI that implement ISaveChangesInterceptor from SaveChangesInterceptor 
                    opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    opt.UseSqlServer(connectionString);
                });
            //add Db context interface to DI
            services.AddScoped<IApplicationDBContext, ApplicationDBContext>();


            return services;
        }

    }
}
