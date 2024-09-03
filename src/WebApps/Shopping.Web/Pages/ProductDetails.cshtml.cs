using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Models.Catalog;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class ProductDetailsModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductDetailsModel> logger)
        : PageModel
    {
        public ProductModel Product { get; set; } = default!;

        [BindProperty]
        public string Color { get; set; } = default!;

        [BindProperty]
        public int Quantity { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(Guid productId)
        {
            var productResponse = await catalogService.GetProduct(productId);
            Product = productResponse.Product;

            return Page();
        }


        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            logger.LogInformation("add to cart button clicked");
            var productResponse = await catalogService.GetProduct(productId);
            var basket = await basketService.LoadBasket();
            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse?.Product?.Name!,
                Quantity = 1,
                Price = productResponse?.Product?.Price ?? 0,
                Color = "Black"
            });
            await basketService.StoreBasket(new StoreBasketRequest(basket));
            return RedirectToPage("Cart");
        }
    }
}
