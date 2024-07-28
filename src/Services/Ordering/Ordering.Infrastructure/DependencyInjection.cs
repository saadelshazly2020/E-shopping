﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            //add db context to services with connection string configuration

            services
                .AddDbContext<ApplicationDBContext>(opt => opt.UseSqlServer(connectionString));

            return services;
        }

    }
}
