using Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Contexts
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<SellerProduct> SellerProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Product");

                builder.HasKey(p => p.Id);

                builder.Property(p => p.Name)
                    .IsRequired();

                builder.HasMany(p => p.Sellers)
                    .WithOne(s => s.Product)
                    .HasForeignKey(s => s.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(p => new { p.Name, p.Brand })
                    .IsUnique()
                    .HasDatabaseName("IX_Products_Name_Brand_Unique");
            });

            modelBuilder.Entity<SellerProduct>(builder =>
            {
                builder.ToTable("SellerProduct");

                builder.HasKey(sp => sp.Id);

                builder.Property(sp => sp.SellerName)
                    .IsRequired();

                builder.Property(sp => sp.SellerProductId)
                    .IsRequired();

                builder.HasIndex(sp => new { sp.ProductId, sp.SellerName })
                    .IsUnique()
                    .HasDatabaseName("IX_SellerProducts_ProductId_SellerName_Unique ");
            });
        }
    }
}