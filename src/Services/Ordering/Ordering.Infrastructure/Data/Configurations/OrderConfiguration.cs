using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>

    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

            //define one-to-many [customer has many orders]
            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired();


            //define many-to-one relation [many orderItems can be in one order] 
            builder.HasMany(x => x.OrderItems).WithOne().HasForeignKey(x => x.OrderId);

            //define OrderName valueObject to be column in order entity with its constraints
            builder.ComplexProperty(x => x.OrderName, builderName =>
                {
                    builderName.Property(x => x.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100).IsRequired();

                }
                );


            //define ShippingAddress valueObject to be columns in order entity with each column constraints
            builder.ComplexProperty(p => p.ShippingAddress, shippingAddressBuilder =>
            {
                shippingAddressBuilder
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

                shippingAddressBuilder
               .Property(x => x.LastName)
               .HasMaxLength(50)
               .IsRequired();

                shippingAddressBuilder
                .Property(x => x.EmailAddress)
                .HasMaxLength(50);

                shippingAddressBuilder
                .Property(x => x.AddressLine)
                .HasMaxLength(180).IsRequired();

                shippingAddressBuilder
                .Property(x => x.Country)
                .HasMaxLength(50);

                shippingAddressBuilder
                .Property(x => x.State)
                .HasMaxLength(50);

                shippingAddressBuilder
               .Property(x => x.ZipCode)
               .HasMaxLength(5).IsRequired();
            });

            //define ShippingAddress valueObject to be columns in order entity with each column constraints
            builder.ComplexProperty(p => p.BillingAddress, billingAddressBuilder =>
            {
                billingAddressBuilder
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

                billingAddressBuilder
               .Property(x => x.LastName)
               .HasMaxLength(50)
               .IsRequired();

                billingAddressBuilder
                .Property(x => x.EmailAddress)
                .HasMaxLength(50);

                billingAddressBuilder
                .Property(x => x.AddressLine)
                .HasMaxLength(180).IsRequired();

                billingAddressBuilder
                .Property(x => x.Country)
                .HasMaxLength(50);

                billingAddressBuilder
                .Property(x => x.State)
                .HasMaxLength(50);

                billingAddressBuilder
               .Property(x => x.ZipCode)
               .HasMaxLength(5).IsRequired();
            });

            //define Payment valueObject to be columns in order entity with each column constraints

            builder.ComplexProperty(x => x.Payment, paymentBuilder =>
            {
                paymentBuilder
                .Property(x => x.CardName)
                .HasMaxLength(50);

                paymentBuilder
               .Property(x => x.CardNumber)
               .HasMaxLength(24).IsRequired();

                paymentBuilder
               .Property(x => x.Expiration)
               .HasMaxLength(10);

                paymentBuilder
               .Property(x => x.CVV)
               .HasMaxLength(3);

                paymentBuilder
               .Property(x => x.PaymentMethod);
            });

            //define enum and db map
            builder.Property(x => x.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(x => x.ToString()
                , status => (OrderStatus)Enum.Parse(typeof(OrderStatus), status));


            builder.Property(x => x.TotalPrice);
        }
    }
}
