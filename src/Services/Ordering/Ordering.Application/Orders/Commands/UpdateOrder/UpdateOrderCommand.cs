﻿using FluentValidation;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsUpdated);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Order Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
    }
}