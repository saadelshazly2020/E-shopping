using Refit;
using Shopping.Web.Models.Catalog;

namespace Shopping.Web.Services
{
    public interface ICatalogService
    {
        [Get("/catalog-service/products?PageNumber={pageNumber}&PageSize={pageSize}")]
        Task<GetProductsResponse> GetProducts(int? pageNumber, int? pageSize);

        [Get("/catalog-service/products/{id}")]
        Task<GetProductByIdResponse> GetProduct(Guid id);

        [Get("/catalog-service/products/Category/{category}")]
        Task<GetProductByCategoryResponse> GetProductByCategory(string category);
    }
}
