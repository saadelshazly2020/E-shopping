
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductByCategory;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByCategoryQueryRequest(string Category);
    public record GetProductByCategoryQueryResponse(IEnumerable<API.Models.Products> Products);
    public class GetProductByCategoryQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",
               async (string category, ISender sender) =>
               {
                   var result = await sender.Send(new GetProductByCategoryQuery(category));
                   if (result.Products==null || result.Products.Count()==0)
                   {
                       return Results.NotFound();
                   }
                   var response = result.Adapt<GetProductByCategoryQueryResponse>();
                   return Results.Ok(response);

               }).WithName("GetProductByCategory")
               .Produces<GetProductByCategoryQueryResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Get Product By Category")
               .WithDescription("Get Product By Category");
        }
    }
}
