
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQueryRequest(Guid Id);
    public record GetProductByIdQueryResponse(API.Models.Products Product);
    public class GetProductByIdQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",
               async (GetProductByIdQueryRequest request, ISender sender) =>
               {
                   var query = request.Adapt<GetProductByIdQuery>();
                   var result = await sender.Send(query);
                   if (result.Product==null)
                   {
                       return Results.NotFound();
                   }
                   var response = result.Adapt<GetProductByIdQueryResponse>();
                   return Results.Ok(response);

               }).WithName("GetProductById")
               .Produces<CreateProductResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Get Product By Id")
               .WithDescription("Get Product By Id");
        }
    }
}
