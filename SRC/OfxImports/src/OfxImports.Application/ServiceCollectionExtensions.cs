using Microsoft.Extensions.Configuration;
using OfxImports.Application.Interfaces;
using OfxImports.Application.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServiceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddInfraDependency(configuration)
                .AddDomainDependency(configuration);

            services.AddTransient<IOfxImportAppService, OfxImportAppService>();

            return services;
        }
    }
}
