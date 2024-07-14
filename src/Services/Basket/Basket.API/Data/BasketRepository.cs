
using BuildingBlocks.Exceptions;
using Marten;

namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            //var basket = await session.Query<ShoppingCart>().Where(x => x.UserName == userName).FirstOrDefaultAsync(cancellationToken);

            //if (basket is null)
            //{
            //    throw new NotFoundException("No Basket to delete");
            //}
            session.Delete<ShoppingCart>(userName);
            await session.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await session.Query<ShoppingCart>().Where(x => x.UserName == userName).FirstOrDefaultAsync(cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException("Basket Not Found");
            }

            return basket;

        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            //var basketData = await session.Query<ShoppingCart>().Where(x => x.UserName == basket.UserName).FirstOrDefaultAsync(cancellationToken);
            //if (basketData is not null)//update
            //{
            //    basketData.Items = basket.Items;

            //}
            //else//add
            //{
            //    basketData = new ShoppingCart { UserName = basket.UserName, Items = basket.Items };
            //}
            session.Store<ShoppingCart>(basket);

            await session.SaveChangesAsync(cancellationToken);
            return basket;

        }
    }
}
