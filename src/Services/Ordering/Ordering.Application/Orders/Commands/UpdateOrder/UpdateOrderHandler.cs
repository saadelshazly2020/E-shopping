﻿namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler(IApplicationDBContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = dbContext.Orders.FirstOrDefault(x => x.Id.Value == command.Order.Id);
            if (order == null)
            {
                throw new OrderNotFoundException("Order", command.Order.Id);
            }
            UpdateOrderWithNewValues(order, command.Order);
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateOrderResult(true);
        }
        private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
        {
            var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(orderDto.BillingAddress.FirstName,
                orderDto.BillingAddress.LastName,
                orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

            var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

            order.Update(
              orderName: OrderName.Of(orderDto.OrderName),
               shippingAddress: shippingAddress,
              billingAddress: billingAddress,
              payment: payment, status: orderDto.Status);
        }
    }
}
