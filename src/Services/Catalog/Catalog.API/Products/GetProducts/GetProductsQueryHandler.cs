
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery : IQuery<GetProductsQueryResult>;
    public record GetProductsQueryResult(IEnumerable<Models.Product> Products);
    public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsQueryResult>
    {
        public async Task<GetProductsQueryResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler.Handle called with {@query}", query);
            var products=await session.Query<Models.Product>().ToListAsync(cancellationToken);
            return new GetProductsQueryResult(products);
        }
    }
}
