﻿// <auto-generated />
using Discount.Grpc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Discount.Grpc.Migrations
{
    [DbContext(typeof(DiscountContext))]
    [Migration("20240719145524_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("Discount.Grpc.Models.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 100m,
                            Description = "SM S24 Ultra Descritpion",
                            ProductName = "SM S24 Ultra"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 50m,
                            Description = "huawei laptop Descritpion",
                            ProductName = "huawei laptop"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
