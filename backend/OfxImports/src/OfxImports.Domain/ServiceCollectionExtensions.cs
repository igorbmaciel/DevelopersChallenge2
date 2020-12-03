using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureMediatR();

            return services;
        }

        private static void ConfigureMediatR(this IServiceCollection services)
        {
        }
    }
}