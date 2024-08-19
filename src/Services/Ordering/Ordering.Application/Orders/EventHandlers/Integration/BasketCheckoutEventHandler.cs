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
                new OrderItemDto(orderId,new Guid("c2bf35be-a7eb-48dd-bd40-2d62e2271439"),2,350),
                new OrderItemDto(orderId,new Guid("6adcf77c-a88f-44eb-a6aa-f59aa3624360"),1,200)
                
            ]);

        return new CreateOrderCommand(orderDto);
    }
}

