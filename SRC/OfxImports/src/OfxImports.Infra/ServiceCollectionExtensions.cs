using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfxImports.Domain.Interfaces;
using OfxImports.Infra.Context;
using OfxImports.Infra.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTnfEntityFrameworkCore()
                .AddTnfDbContext<OfxImportsContext>((config) =>
                {
                    if (config.ExistingConnection != null)
                        config.DbContextOptions.UseNpgsql(config.ExistingConnection);
                    else
                        config.DbContextOptions.UseNpgsql(configuration[$"ConnectionStrings:PostgresSQL"]);
                });         

            //Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            return services;
        }     
    }
}
