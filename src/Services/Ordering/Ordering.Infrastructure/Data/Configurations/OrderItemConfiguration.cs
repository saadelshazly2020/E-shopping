using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(orderItemId => orderItemId.Value, dbId => OrderItemId.Of(dbId));

            //define one to many relationship with product [one product has many orderItems]
            builder.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(x => x.Price).IsRequired();

        }
    }
}
