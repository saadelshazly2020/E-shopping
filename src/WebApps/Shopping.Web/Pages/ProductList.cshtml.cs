using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Models.Catalog;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class ProductListModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductListModel> logger)
        : PageModel
    {

        public IEnumerable<string> CategoryList { get; set; } = [];
        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var result = await catalogService.GetProducts();
            CategoryList = result.Products.SelectMany(x => x.Category).Distinct();

            ProductList = result.Products.Where(x => string.IsNullOrEmpty(categoryName) || x.Category.Contains(categoryName));

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
