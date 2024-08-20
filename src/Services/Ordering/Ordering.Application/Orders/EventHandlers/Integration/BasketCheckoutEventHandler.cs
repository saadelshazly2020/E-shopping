using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.AspNetCore.Components.Web;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler
    (ISender sender, ILogger<BasketCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        //create new order and start order fullfillment process
        logger.LogInformation("Integration Event Handler: {IntegrationEvent}", context.GetType().Name);
        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: OrderStatus.Pending,
            OrderItems:
            [
                new OrderItemDto(orderId,new Guid("40A3D884-6EAE-47C1-B2AE-3E06B8477D0F"),2,350),
                new OrderItemDto(orderId,new Guid("CC52588E-1B7F-4076-9E51-B3944992DD63"),1,200)
                
            ]);

        return new CreateOrderCommand(orderDto);
    }
}

