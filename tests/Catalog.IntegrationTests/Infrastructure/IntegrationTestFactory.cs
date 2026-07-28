using Catalog.Domain.Models;
using Catalog.Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Infrastructure
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _connection;

        public IntegrationTestFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    x => x.ServiceType == typeof(DbContextOptions<CatalogContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<CatalogContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

                var provider = services.BuildServiceProvider();

                using var scope = provider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<CatalogContext>();

                db.Database.EnsureCreated();

                Seed(db);
            });
        }

        private static void Seed(CatalogContext db)
        {
            db.Products.Add(new Product
            {
                Name = "iPhone 15",
                Brand = "Apple",
                Category = "Electronics"
            });

            db.SaveChanges();
        }

        //public async Task ResetDatabaseAsync()
        //{
        //    using var scope = Services.CreateScope();

        //    var db = scope.ServiceProvider.GetRequiredService<CatalogContext>();

        //    await db.Database.EnsureDeletedAsync();
        //    await db.Database.EnsureCreatedAsync();

        //    Seed(db);
        //}
    }
}
