using Catalog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Middlewares
{
    public static class EntityFrameworkConfiguration
    {
        public static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Catalog"));
            });

            return services;
        }
    }
}