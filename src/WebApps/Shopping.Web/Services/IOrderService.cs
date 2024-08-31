using Refit;
using Shopping.Web.Models.Order;

namespace Shopping.Web.Services
{
    public interface IOrderService
    {
        [Get("/order-service/orders?pageIndex={pageIndex}&pageSize={pageSize}")]
        Task<GetOrdersResponse> GetOrders(int? pageIndex = 0, int? pageSize = 10);
        
       
        [Get("/order-service/orders/customer/{customerId}")]
        Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId);

       
        [Get("/order-service/orders/{orderName}")]
        Task<GetOrdersByNameResponse> GetOrdersByName(string orderName);
    }
}
