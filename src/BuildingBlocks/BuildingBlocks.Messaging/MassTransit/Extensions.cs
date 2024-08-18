﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,IConfiguration configuration, Assembly? assembly = null)
        {
            //add message broker configuration
            services.AddMassTransit(config => {

                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null)
                    config.AddConsumers(assembly);

                //add rabbitMQ config
                config.UsingRabbitMq((context, configrator) =>
                {
                    configrator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    configrator.ConfigureEndpoints(context);

                });

                
            });
            return services;
        }
    }
}
