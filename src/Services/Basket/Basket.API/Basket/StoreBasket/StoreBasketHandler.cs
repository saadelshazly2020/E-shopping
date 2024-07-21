
using Basket.API.Data;
using Discount.Grpc.Protos;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StorebasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StorebasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotEmpty().WithMessage("Cart is required");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient protoClient) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {

            await DeductDiscount(request.Cart, cancellationToken);
            //store cart to db
            var basket = await repository.StoreBasket(request.Cart, cancellationToken);
            return new StoreBasketResult(basket.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await protoClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
                item.Price -= coupon.Amount;
            }
        }
    }
}
