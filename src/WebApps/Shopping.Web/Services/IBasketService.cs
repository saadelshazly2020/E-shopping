using Refit;
using Shopping.Web.Models.Basket;

namespace Shopping.Web.Services
{
    public interface IBasketService
    {
        [Get("/basket-service/basket/{userName}")]
        Task<GetBasketResponse> GetBasket(string userName);

        [Post("/basket-service/basket")]
        Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

        [Delete("/basket-service/basket/{userName}")]
        Task<DeleteBasketResponse> DeleteBasket(string UserName);

        [Post("/basket-service/basket/checkout")]
        Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

        public async Task<ShoppingCartModel> LoadBasket()
        {
            var userName = "Saad";
            ShoppingCartModel basket;
            try
            {
                var result = await GetBasket(userName);
                basket = result.Cart;
            }
            catch (Exception)
            {

                basket = new ShoppingCartModel
                {
                    UserName = userName,
                    Items = []
                };
            }

            return basket;

        }
    }
}
