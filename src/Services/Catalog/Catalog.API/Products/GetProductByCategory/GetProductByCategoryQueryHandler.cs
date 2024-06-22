using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductByCategory
{

    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryQueryResult>;
    public record GetProductByCategoryQueryResult(IEnumerable<API.Models.Products> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryQueryResult>
    {
        public async Task<GetProductByCategoryQueryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await session.Query<Models.Products>().Where(x => x.Category.Contains(request.Category)).ToListAsync();
            return new GetProductByCategoryQueryResult(products!);
        }
    }
}
