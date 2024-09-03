using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Order;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class OrderListModel(IOrderService orderService, ILogger<OrderListModel> logger)
        : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = [];
        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Get orders from onGetAsync action");

            var customerId = new Guid("40b9ce16-1e58-49da-8c2b-aaa56f325f26");
            var orderResponse = await orderService.GetOrdersByCustomer(customerId);
            Orders = orderResponse.Orders;
            return Page();
        }


    }
}
