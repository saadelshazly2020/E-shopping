namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDBContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders
                 .Include(x => x.OrderItems)
                 .AsNoTracking()
                 .Where(x => x.OrderName.Value.Contains(request.OrderName))
                 .OrderBy(x => x.OrderName.Value)
                 .ToListAsync(cancellationToken);
            return new GetOrdersByNameResult(orders.ToOrderListDto());
        }
    }
}
