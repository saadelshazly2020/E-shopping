using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class CartModel(IBasketService basketService, ILogger<CartModel> logger) : PageModel
    {
        public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();
        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("get basket");
            var result = await basketService.LoadBasket();
            Cart = result;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(Guid ProductId)
        {
            logger.LogInformation("remove item from cart button clicked");
            var basket = await basketService.LoadBasket();
            basket.Items.RemoveAll(x => x.ProductId == ProductId);
            await basketService.StoreBasket(new StoreBasketRequest(basket));
            return RedirectToPage();
        }
    }
}
