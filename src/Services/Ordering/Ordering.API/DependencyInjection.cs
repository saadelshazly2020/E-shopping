using Carter;
using MediatR;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {

            // add service like carter
            services.AddCarter();
            services.AddLogging();
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            //use service like UseCarter
            app.MapCarter();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            return app;


        }
    }
}
