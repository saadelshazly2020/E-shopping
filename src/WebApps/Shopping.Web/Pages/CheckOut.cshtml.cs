using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Basket;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class CheckOutModel(IBasketService basketService, ILogger<CheckOutModel> logger)
        : PageModel
    {
        [BindProperty]
        public CheckoutBasketModel Order { get; set; } = default!;

        public ShoppingCartModel Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.LoadBasket();

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            logger.LogInformation("Checkout button clicked");
            Cart = await basketService.LoadBasket();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            Order.CustomerId = new Guid("40b9ce16-1e58-49da-8c2b-aaa56f325f26");
            Order.UserName=Cart.UserName;
            Order.TotalPrice = Cart.TotalPrice;
            await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
