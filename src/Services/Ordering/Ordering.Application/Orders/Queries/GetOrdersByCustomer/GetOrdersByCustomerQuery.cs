namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;
public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomeResult>;
public record GetOrdersByCustomeResult(IEnumerable<OrderDto> Orders);