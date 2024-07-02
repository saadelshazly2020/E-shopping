
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber, int? PageSize) : IQuery<GetProductsQueryResult>;
    public record GetProductsQueryResult(IEnumerable<Models.Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
    {
        public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Models.Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
            return new GetProductsQueryResult(products);
        }
    }
}
