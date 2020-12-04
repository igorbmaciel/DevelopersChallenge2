using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OfxImports.Infra.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tnf.Runtime.Session;

namespace Poll.Infra.Context
{
    public class OfxImportsContextFactory : IDesignTimeDbContextFactory<OfxImportsContext>
    {
        public OfxImportsContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OfxImportsContext>();

            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (envName.IsNullOrEmpty())
                envName = "Development";

            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile($"appsettings.{envName}.json", false)
                                    .Build();

            builder.UseNpgsql(configuration["ConnectionStrings:PostgresSQL"]);

            return new OfxImportsContext(builder.Options, NullTnfSession.Instance);
        }
    }
}
