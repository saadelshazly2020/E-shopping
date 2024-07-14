
using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{

    public record GetBasketQuery(string Username):IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);
    public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            //to do get basket from repository
            var basket = await repository.GetBasket(request.Username, cancellationToken);
            return new GetBasketResult(basket);
        }
    }
}
