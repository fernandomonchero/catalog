using Catalog.Application.Interfaces;
using Catalog.Application.UseCases;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Notifications;
using Catalog.Domain.Services;
using Catalog.Infrastructure.Repositories;

namespace Catalog.Api.Middlewares
{
    public static class DependencyInjectionSolver
    {
        public static IServiceCollection SolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<INotificationCollector, NotificationCollector>();
            services.AddScoped<ICatalogImportUseCase, CatalogImportUseCase>();
            services.AddScoped<ICatalogListUseCase, CatalogListUseCase>();
            services.AddScoped<IProductDetailUseCase, ProductDetailUseCase>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISellerProductRepository, SellerProductRepository>();

            return services;
        }
    }
}