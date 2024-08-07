
using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;
public class GetOrdersQueryHandler(IApplicationDBContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var count = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .OrderBy(x => x.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>
               (
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    count: count,
                    data: orders.ToOrderListDto()
                ));



    }
}
