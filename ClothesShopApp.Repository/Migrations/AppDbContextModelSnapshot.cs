﻿// <auto-generated />
using System;
using ClothesShopApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClothesShopApp.Repository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Address", b =>
                {
                    b.Property<int>("addressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("addressId"));

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("addressHeader")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("addresses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("addressId");

                    b.HasIndex("LocationId");

                    b.HasIndex("userId");

                    b.ToTable("addresses");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Category", b =>
                {
                    b.Property<int>("categoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("categoryId"));

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("parentCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("categoryId");

                    b.HasIndex("parentCategoryId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Like", b =>
                {
                    b.Property<int>("likeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("likeId"));

                    b.Property<int>("productId")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("likeId");

                    b.HasIndex("productId");

                    b.HasIndex("userId");

                    b.ToTable("likes");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Location", b =>
                {
                    b.Property<int>("locationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("locationId"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("parentLocationId")
                        .HasColumnType("int");

                    b.HasKey("locationId");

                    b.HasIndex("parentLocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Order", b =>
                {
                    b.Property<int>("orderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("orderId"));

                    b.Property<DateTime>("OrderDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("addressId")
                        .HasColumnType("int");

                    b.Property<decimal>("totalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("orderId");

                    b.HasIndex("addressId");

                    b.HasIndex("userId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.OrderItem", b =>
                {
                    b.Property<int>("orderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("orderItemId"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("orderId")
                        .HasColumnType("int");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("productId")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.HasKey("orderItemId");

                    b.HasIndex("orderId");

                    b.HasIndex("productId");

                    b.ToTable("orderItems");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderStatusId"));

                    b.Property<string>("OrderStatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderStatusId");

                    b.ToTable("orderStatus");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Product", b =>
                {
                    b.Property<int>("productId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("productId"));

                    b.Property<int?>("categoryId")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("stock")
                        .HasColumnType("int");

                    b.HasKey("productId");

                    b.HasIndex("categoryId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.ProductImage", b =>
                {
                    b.Property<int>("productImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("productImageId"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("productImageId");

                    b.HasIndex("ProductId");

                    b.ToTable("productImages");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.ProductSize", b =>
                {
                    b.Property<int>("PrdouctSizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrdouctSizeId"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("PrdouctSizeId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductSize");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.ProductVariant", b =>
                {
                    b.Property<int>("ProductVariantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductVariantId"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("VariantValueId")
                        .HasColumnType("int");

                    b.HasKey("ProductVariantId");

                    b.HasIndex("ProductId");

                    b.HasIndex("VariantValueId");

                    b.ToTable("ProductVariant");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Review", b =>
                {
                    b.Property<int>("reviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("reviewId"));

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("productId")
                        .HasColumnType("int");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("reviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("reviewId");

                    b.HasIndex("productId");

                    b.HasIndex("userId");

                    b.ToTable("reviews");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Size", b =>
                {
                    b.Property<int>("sizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("sizeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("sizeId");

                    b.ToTable("Size");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("userId"));

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("role")
                        .HasColumnType("bit");

                    b.HasKey("userId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Variant", b =>
                {
                    b.Property<int>("VariantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VariantId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VariantId");

                    b.ToTable("Variant");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.VariantValue", b =>
                {
                    b.Property<int>("VariantValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VariantValueId"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.HasKey("VariantValueId");

                    b.HasIndex("VariantId");

                    b.ToTable("VariantValue");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Address", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Location", "Location")
                        .WithMany("addresss")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.User", "user")
                        .WithMany("addresss")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("user");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Category", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Category", "parentCategory")
                        .WithMany("parentIds")
                        .HasForeignKey("parentCategoryId");

                    b.Navigation("parentCategory");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Like", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Product", "product")
                        .WithMany("likes")
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.User", "user")
                        .WithMany("likes")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");

                    b.Navigation("user");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Location", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Location", "parentLocation")
                        .WithMany("subLocations")
                        .HasForeignKey("parentLocationId");

                    b.Navigation("parentLocation");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Order", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Address", "address")
                        .WithMany("orders")
                        .HasForeignKey("addressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.User", "user")
                        .WithMany("orders")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("address");

                    b.Navigation("user");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.OrderItem", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Order", "order")
                        .WithMany("orderItems")
                        .HasForeignKey("orderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.Product", "product")
                        .WithMany("orderItems")
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("order");

                    b.Navigation("product");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Product", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Category", "categories")
                        .WithMany("products")
                        .HasForeignKey("categoryId");

                    b.Navigation("categories");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.ProductImage", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Product", "Product")
                        .WithMany("productImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.ProductSize", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Product", "Product")
                        .WithMany("ProductSizes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.Size", "Size")
                        .WithMany("ProductSizes")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.ProductVariant", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Product", "Product")
                        .WithMany("ProductVariants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.VariantValue", "VariantValue")
                        .WithMany("ProductVariant")
                        .HasForeignKey("VariantValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("VariantValue");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Review", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Product", "product")
                        .WithMany("reviews")
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClothesShopApp.Data.Entity.User", "user")
                        .WithMany("reviews")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");

                    b.Navigation("user");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.VariantValue", b =>
                {
                    b.HasOne("ClothesShopApp.Data.Entity.Variant", "Variant")
                        .WithMany("VariantValues")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Address", b =>
                {
                    b.Navigation("orders");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Category", b =>
                {
                    b.Navigation("parentIds");

                    b.Navigation("products");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Location", b =>
                {
                    b.Navigation("addresss");

                    b.Navigation("subLocations");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Order", b =>
                {
                    b.Navigation("orderItems");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Product", b =>
                {
                    b.Navigation("ProductSizes");

                    b.Navigation("ProductVariants");

                    b.Navigation("likes");

                    b.Navigation("orderItems");

                    b.Navigation("productImages");

                    b.Navigation("reviews");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Size", b =>
                {
                    b.Navigation("ProductSizes");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.User", b =>
                {
                    b.Navigation("addresss");

                    b.Navigation("likes");

                    b.Navigation("orders");

                    b.Navigation("reviews");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.Variant", b =>
                {
                    b.Navigation("VariantValues");
                });

            modelBuilder.Entity("ClothesShopApp.Data.Entity.VariantValue", b =>
                {
                    b.Navigation("ProductVariant");
                });
#pragma warning restore 612, 618
        }
    }
}
