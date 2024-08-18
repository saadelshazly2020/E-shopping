using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Orders.EventHandlers;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //add services here 
            //services.AddScoped<INotificationHandler<OrderUpdatedEvent>, OrderUpdatedEventHandler>();
            //services.AddScoped<INotificationHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();
            services.AddLogging();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());//register mediator service from current assembly
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            // add message broker with config to consume checkout event from rabbitMQ
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

            return services;

        }
    }
}
