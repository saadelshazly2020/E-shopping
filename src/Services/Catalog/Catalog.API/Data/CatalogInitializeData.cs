using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitializeData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<Product>().AnyAsync())
                return;

            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync();
        }

        public static IEnumerable<Product> GetPreConfiguredProducts() => new Product[] {
            new Product
            {
                Id= Guid.NewGuid(),
                Category=["category1","Category2"],
                Description="description1",
                ImageFile="img-1.png",
                Name="product1",
                Price=119
            },
            new Product
            {
                Id=  Guid.NewGuid(),
                Category=["category3","Category4"],
                Description="description2",
                ImageFile="img-2.png",
                Name="product2",
                Price=300
            }
            ,
            new Product
            {
                Id=  Guid.NewGuid(),
                Category=["category5","Category6"],
                Description="description3",
                ImageFile="img-3.png",
                Name="product3",
                Price=500
            }

        };
    }
}
