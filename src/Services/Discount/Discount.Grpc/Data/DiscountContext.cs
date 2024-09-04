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
                ProductName= "IPhone X",
                Description= "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Amount=100
            },
            new Coupon
            {
                Id = 2,
                ProductName = "Xiaomi Mi 9",
                Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Amount = 50
            }
            );
        }
    }
}
