

using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
            //use feature managment flag here to enable or disable publish order created event to rabbitMQ
            if (await featureManager.IsEnabledAsync("OrderFulfilment"))
            {
                //map to orderdto as publish message of massTransit
                var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
                //publish integration event to rabbitMQ using massTransit
                await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            }

        }
    }
}
