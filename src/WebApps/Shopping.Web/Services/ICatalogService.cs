using Shopping.Web.Models.Catalog;

namespace Shopping.Web.Services
{
    public interface ICatalogService
    {
        Task<GetProductsResponse> GetProducts(int? PageNumber, int? PageSize);
        Task<GetProductByIdResponse> GetProduct(Guid Id);
        Task<GetProductByCategoryResponse> GetProductByCategory(string Category);
    }
}
