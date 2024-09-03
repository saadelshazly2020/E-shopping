using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Models.Catalog;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class IndexModel(IBasketService basketService, ICatalogService catalogService, ILogger<IndexModel> logger) : PageModel
    {

        public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();



        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Index OnGetAsync");
            var result = await catalogService.GetProducts();
            ProductList = result.Products;
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