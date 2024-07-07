
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart):ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StorebasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StorebasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotEmpty().WithMessage("Cart is required");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {   
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            //store cart to db

            return new StoreBasketResult("swn");
        }
    }
}
