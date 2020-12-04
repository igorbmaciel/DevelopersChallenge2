using Microsoft.EntityFrameworkCore;
using OfxImports.Domain.Entities;
using OfxImports.Infra.Mappers;
using Tnf.EntityFrameworkCore;
using Tnf.Runtime.Session;

namespace OfxImports.Infra.Context
{
    public class OfxImportsContext : TnfDbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public OfxImportsContext(DbContextOptions<OfxImportsContext> options, ITnfSession session)
           : base(options, session)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyTableNameToLowerConventions(modelBuilder);

            modelBuilder.ApplyConfiguration(new BankAccountMapper());
            modelBuilder.ApplyConfiguration(new TransactionMapper());

            base.OnModelCreating(modelBuilder);
        }

        private static void ApplyTableNameToLowerConventions(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.Relational().TableName.ToLower();

                foreach (var property in entity.GetProperties())
                    property.Relational().ColumnName = property.Relational().ColumnName.ToLower();

                foreach (var key in entity.GetKeys())
                    key.Relational().Name = key.Relational().Name.ToLower();

                foreach (var key in entity.GetForeignKeys())
                    key.Relational().Name = key.Relational().Name.ToLower();

                foreach (var index in entity.GetIndexes())
                    index.Relational().Name = index.Relational().Name.ToLower();
            }
        }
    }
}
