using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtension
    {
        public async static Task InitializeDatabaseAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            await context.Database.MigrateAsync();

            await SeedDataAsync(context);


        }

        private static async Task SeedDataAsync(ApplicationDBContext context)
        {
            await SeedCustomerDataAsync(context);
            await SeedProductDataAsync(context);
            await SeedOrderWithItemsDataAsync(context);
        }



        private static async Task SeedCustomerDataAsync(ApplicationDBContext context)
        {
            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedProductDataAsync(ApplicationDBContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedOrderWithItemsDataAsync(ApplicationDBContext context)
        {
            if (!await context.Orders.AnyAsync())
            {
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                await context.SaveChangesAsync();
            }
        }
    }
}
