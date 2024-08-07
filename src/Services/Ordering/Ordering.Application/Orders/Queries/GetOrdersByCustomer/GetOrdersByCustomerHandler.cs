namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerHandler(IApplicationDBContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomeResult>
    {
        public async Task<GetOrdersByCustomeResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .Where(x => x.CustomerId == CustomerId.Of(query.CustomerId))
                .OrderBy(x => x.OrderName.Value)
                .ToListAsync(cancellationToken);
            return new GetOrdersByCustomeResult(orders.ToOrderListDto());
        }
    }
}
