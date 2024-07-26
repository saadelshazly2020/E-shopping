using MediatR;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {

            // add service like carter
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            //use service like UseCarter


            return app;


        }
    }
}
