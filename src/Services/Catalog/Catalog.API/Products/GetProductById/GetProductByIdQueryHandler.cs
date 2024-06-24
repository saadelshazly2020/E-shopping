using Catalog.API.Models;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductById
{

    public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdQueryResult>;
    public record GetProductByIdQueryResult(API.Models.Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult>
    {
        public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Models.Product>(request.Id);
            return new GetProductByIdQueryResult(product!);
        }
    }
}
