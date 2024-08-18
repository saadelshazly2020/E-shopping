using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(CheckoutBasketDto CheckoutBasketDto) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);

    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.CheckoutBasketDto).NotNull().WithMessage("CheckoutBasketDto is required");
            RuleFor(x => x.CheckoutBasketDto.UserName).NotNull().WithMessage("UserName is required");
        }
    }

    public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            //get basket from db
            var basket = await repository.GetBasket(command.CheckoutBasketDto.UserName, cancellationToken);
            if (basket == null)
            {
                return new CheckoutBasketResult(false);
            }
            //map and set total price to basket checkout event message
            var eventMessage = command.CheckoutBasketDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            //publish the basket check event to rabbitMQ 
            await publishEndpoint.Publish(eventMessage, cancellationToken);
            //delete basket
            await repository.DeleteBasket(command.CheckoutBasketDto.UserName, cancellationToken);
            return new CheckoutBasketResult(true);
        }
    }
}
