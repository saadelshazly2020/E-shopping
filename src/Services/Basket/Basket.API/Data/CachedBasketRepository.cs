
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    /// <summary>
    ///     here we used two patterns
    ///     1. the first one is proxy pattern:
    ///        where the CachedBasketRepository act as proxy where it takes the request 
    ///        and pass it to IBasketRepository.
    ///     2. the second is the decorator parttern:
    ///        where the CachedBasketRepository implements and  extends IBasketRepository
    ///        and add caching logic.
    /// </summary>
    /// <param name="repository"></param>
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {


        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            //check if the basket in cache then return 
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            //if not in cache get it from db and save to cache and then return it to caller
            var basket = await repository.GetBasket(userName, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize<ShoppingCart>(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            //store first on repo and then ion cache
            await repository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            //remove first from repo rtrhen from cache
            await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);
            return true;
        }
    }
}
