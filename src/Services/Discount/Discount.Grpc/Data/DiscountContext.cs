using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; }

        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id=1,
                ProductName= "SM S24 Ultra",
                Description= "SM S24 Ultra Descritpion",
                Amount=100
            },
            new Coupon
            {
                Id = 2,
                ProductName = "huawei laptop",
                Description = "huawei laptop Descritpion",
                Amount = 50
            }
            );
        }
    }
}
