using Microsoft.EntityFrameworkCore;
using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        DbSet<Category> categories { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<User> users { get; set; }
        DbSet<Review> reviews { get; set; }
        DbSet<ProductImage> productImages { get; set; }
        DbSet<Product> products { get; set; }
        DbSet<OrderItem> orderItems { get; set; }
        DbSet<Order> orders { get; set; }
        DbSet<OrderStatus> orderStatus { get; set; }
        DbSet<Like> likes { get; set; }
        DbSet<Address> addresses { get; set; }
        DbSet<Variant> Variant { get; set; }
        DbSet<VariantValue> VariantValue { get; set; }
        DbSet<ProductVariant> ProductVariant { get; set; }
    }
}
