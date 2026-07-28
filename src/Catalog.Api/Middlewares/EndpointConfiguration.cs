namespace Catalog.Api.Middlewares
{
    public static class EndpointConfiguration
    {
        public static IServiceCollection ConfigureEndpoints(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}